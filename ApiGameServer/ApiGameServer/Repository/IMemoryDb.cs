namespace ApiGameServer.Repository;

public interface IMemoryDb
{
    public Task<ErrorCode> SetAccessTokenAsync(string id, string authToken);
    public Task<ErrorCode> VerifyAccessTokenAsync(string id, string authToken);
}
