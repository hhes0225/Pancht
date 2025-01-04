using ApiGameServer.Models.DTO;
using ApiGameServer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiGameServer.Controllers;

public class CheckMatchingController : Controller
{
    ILogger<CheckMatchingController> _logger;
    IMatchingService _service;

    public CheckMatchingController(ILogger<CheckMatchingController> logger, IMatchingService matchingService)
    {
        _logger = logger;
        _service = matchingService;
    }


    [HttpPost]
    public async Task<CheckMatchingResponse> CheckMatching([FromBody] CheckMatchingRequest request)
    {
        CheckMatchingResponse response = new CheckMatchingResponse();
        //MatchingService를 통해 매칭 요청
        response = await _service.CheckMatchingAsync(request);
        return response;
    }
}
