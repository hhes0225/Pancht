using ApiGameServer.Models.DTO;

namespace ApiGameServer.Service;

public interface ICreateUserService
{
    public Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);
    
}
