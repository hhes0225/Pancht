namespace ApiGameServer.Repository;

public interface IUserStateDb
{
    public Task<ErrorCode> CreateUserStateAsync(string id);
    public Task<ErrorCode> SetUserStateToMatchingAsync(string id);
}
