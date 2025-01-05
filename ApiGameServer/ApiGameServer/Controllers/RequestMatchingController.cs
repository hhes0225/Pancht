using ApiGameServer.Models.DTO;
using ApiGameServer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace ApiGameServer.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestMatchingController : ControllerBase
{
    ILogger<RequestMatchingController> _logger;
    IMatchingService _service;

    public RequestMatchingController(ILogger<RequestMatchingController> logger, IMatchingService matchingService)
    {
        _logger = logger;
        _service = matchingService;
    }

    [HttpPost]
    public async Task<MatchingResponse> Matching([FromBody] MatchingRequestFromClient request)
    {
        MatchingResponse response = new MatchingResponse();
        //MatchingService를 통해 매칭 요청
        response = await _service.RequestMatchingAsync(request);
        return response;
    }

}
