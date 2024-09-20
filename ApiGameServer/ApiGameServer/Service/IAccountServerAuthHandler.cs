namespace ApiGameServer.Service;

public interface IAccountServerAuthHandler
{
    public Task<ErrorCode> RequestVerifyToken(string id, string authToken);
}
