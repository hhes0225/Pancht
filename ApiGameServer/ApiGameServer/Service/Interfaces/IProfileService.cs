using ApiGameServer.Models.DTO;

namespace ApiGameServer.Service.Interfaces;

public interface IProfileService
{
    public Task<ProfileResponse> GetProfileAsync(ProfileRequest request);
}
