using ApiAccountServer.Models.DTO;

namespace ApiAccountServer.Service;

public interface IVerifyTokenService
{
    public Task<VerifyTokenResponse> VerifyTokenAsync(VerifyTokenRequest request);
}
