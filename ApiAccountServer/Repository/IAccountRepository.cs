namespace ApiAccountServer.Repository;

public interface IAccountRepository
{
    public Task<ErrorCode> InsertAccountAsync(string id, string pw);
    public Task<string> FindUserById(string id);
    public Task<ErrorCode> VerifyUser(string id, string pw);
}
