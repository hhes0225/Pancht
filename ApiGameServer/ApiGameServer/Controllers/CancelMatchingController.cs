using ApiGameServer.Models.DTO;
using ApiGameServer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiGameServer.Controllers;

public class CancelMatchingController : Controller
{
    ILogger<CancelMatchingController> _logger;
    IMatchingService _service;

    public CancelMatchingController(ILogger<CancelMatchingController> logger, IMatchingService matchingService)
    {
        _logger = logger;
        _service = matchingService;
    }

    [HttpPost]
    public async Task<CancelMatchingResponse> CancelMatching([FromBody] CancelMatchingRequest request)
    {
        CancelMatchingResponse response = new CancelMatchingResponse();
        //MatchingService를 통해 매칭 요청
        response = await _service.CancelMatchingAsync(request);
        return response;
    }

}
