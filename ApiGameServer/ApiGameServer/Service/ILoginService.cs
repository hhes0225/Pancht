using ApiGameServer.Models.DAO;
using ApiGameServer.Models.DTO;

namespace ApiGameServer.Service;

public interface ILoginService
{
    public Task<LoginResponse> LoginAsync(LoginRequest request);
    public Task<HttpResponseMessage> SendVerifyRequestAsync(string id, string authToken);
}
