using ApiGameServer.Models.DAO;

namespace ApiGameServer.Repository;

public interface IPanchtDb
{
    public Task<(ErrorCode, UserData)> CreateUserDataAsync(string userId, string nickname);
    public Task<(ErrorCode, UserData)> GetUserDataAsync(string id);
    public Task<bool> CheckNicknameExistAsync(string nickname);
    public Task<(ErrorCode, List<UserCharacterData>)> GetUserCharacterDataAsync(string userId);

    public Task<(ErrorCode, AttendanceData)> GetAttendanceDataAsync(string id);
    public Task<ErrorCode> UpdateAttendanceDataAsync(AttendanceData item2);

    //매칭을 위한 유저 데이터 조회
    public Task<(ErrorCode, int)> GetUserTierScoreAsync(string id);
    public Task<(ErrorCode, GameResult)> GetLastGameResultAsync(string id);
}
