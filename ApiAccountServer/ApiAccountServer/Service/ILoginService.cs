using ApiAccountServer.Models.DTO;

namespace ApiAccountServer.Service;

public interface ILoginService
{
    public Task<LoginResponse> LoginAsync(LoginRequest request);

}
