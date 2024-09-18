using ApiAccountServer.Models.DTO;
using ApiAccountServer.Repository;

namespace ApiAccountServer.Service;

public class VerifyTokenService:IVerifyTokenService
{

    ILogger<VerifyTokenService> _logger;
    IMemoryDb _memoryRepository;

    public VerifyTokenService(ILogger<VerifyTokenService> logger, IMemoryDb repository)
    {
        _logger = logger;
        _memoryRepository = repository;
    }

    //Redis에서 토큰 검증
    public async Task<VerifyTokenResponse> VerifyTokenAsync(VerifyTokenRequest request)
    {
        var result = await _memoryRepository.GetAccessToken(request.Id, request.AuthToken);

        if (result != ErrorCode.None)
        {
            return new VerifyTokenResponse { Result = result };
        }

        return new VerifyTokenResponse { Result = result };

    }
}
