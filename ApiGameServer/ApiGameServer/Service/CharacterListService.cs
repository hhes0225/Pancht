using ApiGameServer.Models.DTO;
using ApiGameServer.Repository;
using ApiGameServer.Service.Interfaces;

namespace ApiGameServer.Service;

public class CharacterListService:ICharacterListService
{
    ILogger<CharacterListService> _logger;
    readonly IPanchtDb _panchtDb;

    public CharacterListService(ILogger<CharacterListService> logger, IPanchtDb userDataDb)
    {
        _logger = logger;
        _panchtDb = userDataDb;
    }

    public async Task<CharacterListResponse> CharacterListAsync(CharacterListRequest request)
    {
        var characterListResponse = new CharacterListResponse();

        //캐릭터 리스트 조회
        var result = await _panchtDb.GetUserCharacterDataAsync(request.Id);

        _logger.LogInformation($"캐릭터 리스트 조회 결과: {result.Item1}");


        if (result.Item1 != ErrorCode.None)
        {
            _logger.LogError($"GetCharacterListAsync Error: {result.Item1}");
            characterListResponse.Result = result.Item1;
            return characterListResponse;
        }

        _logger.LogInformation("GetCharacterListAsync Success");

        characterListResponse.UserCharacterList = result.Item2;

        _logger.LogInformation($"CharacterList: {characterListResponse.UserCharacterList}");

        return characterListResponse;
    }
}
