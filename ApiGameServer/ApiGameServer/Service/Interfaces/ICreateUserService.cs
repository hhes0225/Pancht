using ApiGameServer.Models.DTO;

namespace ApiGameServer.Service.Interfaces;

public interface ICreateUserService
{
    public Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);

}
