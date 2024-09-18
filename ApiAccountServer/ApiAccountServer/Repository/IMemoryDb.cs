namespace ApiAccountServer.Repository;

public interface IMemoryDb
{
    public Task<ErrorCode> SetAccessToken(string id, string authToken);
    public Task<ErrorCode> GetAccessToken(string id, string authToken);

}
