using ApiAccountServer.Models.DTO;
using ApiAccountServer.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiAccountServer.Controllers;

[ApiController]
[Route(("[controller]"))]
public class RegisterController : ControllerBase
{
    //Service
    IRegisterService _service;
    ILogger<RegisterController> _logger;
    //zlogger 사용해보기


    public RegisterController(IRegisterService service, ILogger<RegisterController> logger)
    {
        _service = service;
        _logger = logger;
    }


    [HttpPost]
    public async Task<RegisterResponse> Register([FromBody]RegisterRequest request)
    {
        RegisterResponse response = new RegisterResponse();

        //Repository의 Resgister Account 함수 호출(비동기적으로)
        response.Result = await _service.RegisterAsync(request);

        _logger.LogInformation($"Register Result: {response.Result}");

        return response;
    }
}
