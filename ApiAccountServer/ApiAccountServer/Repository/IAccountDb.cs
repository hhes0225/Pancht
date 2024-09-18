namespace ApiAccountServer.Repository;

public interface IAccountDb
{
    public Task<ErrorCode> InsertAccountAsync(string id, string pw);
    public Task<string> FindUserById(string id);
    public Task<ErrorCode> VerifyUserLogin(string id, string pw);
}
