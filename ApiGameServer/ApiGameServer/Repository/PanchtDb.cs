using Microsoft.Extensions.Options;
using System.Data;
using MySqlConnector;
using SqlKata.Execution;
using ApiGameServer.Models.DAO;

namespace ApiGameServer.Repository;

public class PanchtDb:IPanchtDb, IDisposable
{
    ILogger<PanchtDb> _logger;
    readonly IOptions<DbConfig> _dbConfig;
    IDbConnection _dbConnection;
    readonly SqlKata.Compilers.MySqlCompiler _compiler;
    readonly QueryFactory _queryFactory;

    public PanchtDb(ILogger<PanchtDb> logger, IOptions<DbConfig> dbConfig)
    {
        _logger = logger;
        _dbConfig = dbConfig;

        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConnection, _compiler);
    }

    public void Dispose()
    {
        Close();
    }

    //유저 데이터 생성
    public async Task<(ErrorCode, UserData)> CreateUserDataAsync(string userId, string nickname)
    {
        var newUser = new UserData
        {
            id = userId,
            nickname = nickname,
            create_date = DateTime.Now,
            total_games = 0,
            win_count = 0,
            draw_count = 0,
            lose_count = 0,
            tier_score = 0
        };

        try
        {
            var result = await _queryFactory.Query("UserData").InsertAsync(newUser);

            if(result == 0)
            {
                return (ErrorCode.GameDataCreateFailException, newUser);
            }


        }
        catch (Exception e)
        {
            _logger.LogError(e, "CreateUserDataAsync Error");
            return (ErrorCode.GameDataCreateFailException, newUser);
        }

        return (ErrorCode.None, newUser);
    }

    public async Task<(ErrorCode, AttendanceData)> CreateAttendanceDataAsync(Int64 userId)
    {
        var newAttendanceData = new AttendanceData
        {
            uid = userId,
            last_attendance_date = null,
            attendance_count = 0
        };

        try
        {
            var result = await _queryFactory.Query("AttendanceData").InsertAsync(newAttendanceData);

            if (result == 0)
            {
                return (ErrorCode.AttendanceDataCreateFailException, newAttendanceData);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CreateAttendanceDataAsync Error");
            return (ErrorCode.AttendanceDataCreateFailException, newAttendanceData);
        }

        return (ErrorCode.None, newAttendanceData);
    }

    //유저 데이터 조회
    public async Task<(ErrorCode, UserData)> GetUserDataAsync(string id)
    {
        try
        {
            var user = await _queryFactory.Query("UserData").Where("id", id).FirstOrDefaultAsync<UserData>();

            return (ErrorCode.None, user);
        }
        catch(Exception e)
        {
            _logger.LogError(e, "GetUserDataAsync Error");
            return (ErrorCode.GameDataLoadException, null);
        }
    }

    public async Task<Int64> GetUidById(string id)
    {
        try
        {
            var user = await _queryFactory.Query("UserData").Where("id", id).FirstOrDefaultAsync<UserData>();

            return user.uid;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetUserDataAsync Error");
            return -1;
        }
    }

    //닉네임 DB에 존재하는지 확인
    public async Task<bool> CheckNicknameExistAsync(string nickname)
    {
        try
        {
            var user = await _queryFactory.Query("UserData").Where("nickname", nickname).FirstOrDefaultAsync<UserData>();

            if(user != null)
            {
                return true;
            }
        }
        catch(Exception e)
        {
            _logger.LogError(e, "CheckNicknameExist Error");
            return false;
        }

        return false;
    }

    //유저가 수집한 캐릭터 데이터 조회
    public async Task<(ErrorCode, List<UserCharacterData>)> GetUserCharacterDataAsync(string id)
    {
        try
        {
            var uid = await GetUidById(id);

            if(uid == -1)
            {
                return (ErrorCode.GameDataLoadException, null);
            }

            var userCharacterData = await _queryFactory.Query("User_Character").Where("user_id", uid).GetAsync<UserCharacterData>();

            if( userCharacterData == null )
            {
                return (ErrorCode.GameCharacterDataLoadFail, null);
            }

            if (!userCharacterData.Any()) 
            {
                return (ErrorCode.GameCharacterDataNotExist, userCharacterData.ToList());
            }

            return (ErrorCode.None, userCharacterData.ToList());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetUserCharacterDataAsync Error");
            return (ErrorCode.GameDataLoadException, null);
        }
    }

    //출석체크 데이터 조회
    public async Task<(ErrorCode, AttendanceData)> GetAttendanceDataAsync(string id)
    {
        try
        {
            var attendanceData = await _queryFactory.Query("AttendanceData").Where("id", id).FirstOrDefaultAsync<AttendanceData>();

            if(attendanceData == null)
            {
                attendanceData = new AttendanceData
                {
                    uid = 0,
                    last_attendance_date = null,
                    attendance_count = 0
                };

                return (ErrorCode.AttendanceDataNotExist, attendanceData);
            }

            return (ErrorCode.None, attendanceData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetAttendanceDataAsync Error");
            return (ErrorCode.AttendanceDataLoadFail, null);
        }
    }

    //출석체크 데이터 갱신
    public async Task<ErrorCode> UpdateAttendanceDataAsync(AttendanceData attendanceData)
    {
        try
        {
            var result = await _queryFactory.Query("AttendanceData").Where("uid", attendanceData.uid).UpdateAsync(new
            {
                last_attendance_date = attendanceData.last_attendance_date,
                attendance_count = attendanceData.attendance_count
            });

            if (result == 0)
            {
                return ErrorCode.AttendanceDataUpdateFailException;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "UpdateAttendanceDataAsync Error");
            return ErrorCode.AttendanceDataUpdateFailException;
        }

        return ErrorCode.None;
    }

    //매칭을 위한 유저 데이터 조회
    public async Task<(ErrorCode, int)> GetUserTierScoreAsync(string id)
    {
        try
        {
            var user = await _queryFactory.Query("UserData").Where("id", id).FirstOrDefaultAsync<UserData>();
            if (user == null)
            {
                return (ErrorCode.GameDataLoadException, 0);
            }
            return (ErrorCode.None, user.tier_score);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetUserTierScore Error");
            return (ErrorCode.GameDataLoadException, 0);
        }
    }

    public async Task<(ErrorCode, GameResult)> GetLastGameResultAsync(string id)
    {
        try
        { 
            //존재하는 유저인지 확인
            var user = GetUserDataAsync(id);

            if(user.Result.Item1 != ErrorCode.None)
            {
                return (ErrorCode.GameDataLoadException, GameResult.None);
            }

            //게임 매칭 히스토리 존재하는지 확인
            var gameResult = await _queryFactory.
                Query("MatchingHistory").
                OrderByDesc("match_date").
                Where("user_id", user.Result.Item2.uid).
                FirstOrDefaultAsync<MatchingHistoryData>();

            if(gameResult == null)
            {
                return (ErrorCode.None, GameResult.None);
            }

            return (ErrorCode.None, gameResult.result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetGameResult Error");
            return (ErrorCode.GameDataLoadException, GameResult.None);
        }
    }

    private void Open()
    {
        _dbConnection = new MySqlConnection(_dbConfig.Value.MySqlPanchtDb);
        _dbConnection.Open();
    }

    private void Close()
    {
        _dbConnection.Close();
    }
}
