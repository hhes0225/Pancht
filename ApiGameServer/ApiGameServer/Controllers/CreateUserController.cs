using ApiGameServer.Models.DTO;
using ApiGameServer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiGameServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateUserController:ControllerBase
{
    readonly ILogger<CreateUserController> _logger;
    readonly ICreateUserService _service;

    public CreateUserController(ILogger<CreateUserController> logger, ICreateUserService createUserService)
    {
        _logger = logger;
        _service = createUserService;
    }

    [HttpPost]
    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        var response = new CreateUserResponse();

        //CreateUserService를 통해 유저 생성 요청
        response = await _service.CreateUserAsync(request);

        return response;
    }
}
