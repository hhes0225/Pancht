namespace ApiGameServer.Service.Interfaces;

public interface IAccountServerAuthHandler
{
    public Task<ErrorCode> RequestVerifyToken(string id, string authToken);
}
