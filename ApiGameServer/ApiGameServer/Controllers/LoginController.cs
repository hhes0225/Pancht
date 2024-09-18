using ApiGameServer.Models.DTO;
using ApiGameServer.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiGameServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    ILogger<LoginController> _logger;
    ILoginService _service;

    public LoginController(ILogger<LoginController> logger, ILoginService loginService)
    {
        _logger = logger;
        _service = loginService;
    }


    [HttpPost]
    public async Task<LoginResponse> Login([FromBody] LoginRequest request)
    {
        LoginResponse response = new LoginResponse();

        //LoginService를 통해 로그인 요청
        response = await _service.LoginAsync(request);

        return response;
    }
}
