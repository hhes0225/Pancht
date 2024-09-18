using ApiAccountServer.Models.DTO;
using ApiAccountServer.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiAccountServer.Controllers;

[ApiController]
[Route("[controller]")]
public class VerifyTokenController : ControllerBase
{
    ILogger<VerifyTokenController> _logger;
    IVerifyTokenService _service;

    public VerifyTokenController(ILogger<VerifyTokenController> logger, IVerifyTokenService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    public async Task<VerifyTokenResponse> VerifyToken([FromBody] VerifyTokenRequest request)
    {
        VerifyTokenResponse response = new VerifyTokenResponse();

        //Repository의 VerifyToken 함수 호출(비동기적으로)
        response = await _service.VerifyTokenAsync(request);

        return response;
    }

}
