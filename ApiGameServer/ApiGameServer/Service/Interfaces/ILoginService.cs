using ApiGameServer.Models.DAO;
using ApiGameServer.Models.DTO;

namespace ApiGameServer.Service.Interfaces;

public interface ILoginService
{
    public Task<LoginResponse> LoginAsync(LoginRequest request);
}
