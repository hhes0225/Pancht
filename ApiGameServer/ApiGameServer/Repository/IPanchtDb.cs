using ApiGameServer.Models.DAO;

namespace ApiGameServer.Repository;

public interface IPanchtDb
{
    public Task<(ErrorCode, UserData)> CreateUserDataAsync(string userId, string nickname);
    public Task<(ErrorCode, UserData)> GetUserDataAsync(string id);
    public Task<int> GetTierIdByTierScore(int tierScore);
    public Task<bool> CheckNicknameExist(string nickname);
}
