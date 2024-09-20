using Microsoft.Extensions.Options;
using System.Data;
using MySqlConnector;
using SqlKata.Execution;
using ApiGameServer.Models.DAO;

namespace ApiGameServer.Repository;

public class PanchtDb:IPanchtDb
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

    public async Task<(ErrorCode, UserData)> CreateUserDataAsync(string userId, string nickname)
    {
        var newUser = new UserData
        {
            id = userId,
            nickname = nickname,
            create_date = DateTime.Now,
            tier_id = 10,
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
            return (ErrorCode.GameDataLoadFleException, null);
        }
    }

    public async Task<int> GetTierIdByTierScore(int tierScore)
    {
        var tierId = 0;
        try
        {
            tierId = await _queryFactory.Query("Tier").Select("tier_id").
                Where("min_score", "<=", tierScore).Where("max_score", ">=", tierScore).FirstAsync<int>();
        }
        catch(Exception e)
        {
            _logger.LogError(e, "GetTierByTierScore Error");
            return 0;
        }

        return tierId;
    }

    public async Task<bool> CheckNicknameExist(string nickname)
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
