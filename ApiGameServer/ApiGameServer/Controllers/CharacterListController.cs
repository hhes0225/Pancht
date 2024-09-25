using ApiGameServer.Models.DTO;
using ApiGameServer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiGameServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CharacterListController : ControllerBase
{
    ILogger<CharacterListController> _logger;
    ICharacterListService _service;

    public CharacterListController(ILogger<CharacterListController> logger, ICharacterListService characterListService)
    {
        _logger = logger;
        _service = characterListService;
    }

    [HttpPost]
    public async Task<CharacterListResponse> CharacterList([FromBody] CharacterListRequest request)
    {
        CharacterListResponse response = new CharacterListResponse();

        //CharacterListService를 통해 캐릭터 리스트 요청
        response = await _service.CharacterListAsync(request);

        return response;
    }
}
