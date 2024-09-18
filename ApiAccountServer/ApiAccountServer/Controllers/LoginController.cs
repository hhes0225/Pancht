using Microsoft.AspNetCore.Mvc;
using ApiAccountServer.Service;
using ApiAccountServer.Models.DTO;

namespace ApiAccountServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    ILogger<LoginController> _logger;
    ILoginService _service;

    public LoginController(ILogger<LoginController> logger, ILoginService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    public async Task<LoginResponse> Login([FromBody]LoginRequest request)
    {
        LoginResponse response = new LoginResponse();

        //Repository의 Login Account 함수 호출(비동기적으로)
        response = await _service.LoginAsync(request);

        return response;
    }
}
