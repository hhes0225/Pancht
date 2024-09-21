using ApiGameServer.Models.DTO;
using ApiGameServer.Repository;
using ApiGameServer.Service.Interfaces;

namespace ApiGameServer.Service;

public class ProfileService:IProfileService
{
    ILogger<ProfileService> _logger;
    readonly IPanchtDb _panchtDb;
    
    public ProfileService(ILogger<ProfileService> logger, IPanchtDb userDataDb)
    {
        _logger = logger;
        _panchtDb = userDataDb;
    }

    public async Task<ProfileResponse> GetProfileAsync(ProfileRequest request)
    {
        var profileResponse = new ProfileResponse();

        //유저 데이터 조회
        var result = await _panchtDb.GetUserDataAsync(request.Id);

        if (result.Item1 != ErrorCode.None)
        {
            _logger.LogError($"GetUserDataAsync Error: {result.Item1}");
            profileResponse.Result = result.Item1;
            return profileResponse;
        }

        _logger.LogInformation("GetUserDataAsync Success");

        profileResponse.UserGameData = result.Item2;

        _logger.LogInformation($"UserGameData: {profileResponse.UserGameData}");

        return profileResponse;
    }
}
