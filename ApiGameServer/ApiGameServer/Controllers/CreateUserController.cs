using ApiGameServer.Models.DTO;
using ApiGameServer.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ApiGameServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreateUserController:ControllerBase
{
    readonly ILogger<CreateUserController> _logger;
    readonly IPanchtDb _userDataDb;
    readonly IMemoryDb _memoryDb;

    public CreateUserController(ILogger<CreateUserController> logger, IPanchtDb userDataDb, IMemoryDb memoryDb)
    {
        _logger = logger;
        _userDataDb = userDataDb;
        _memoryDb = memoryDb;
    }

    [HttpPost]
    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        var response = new CreateUserResponse();
        //var (errorCode, userData) = await _userDataDb.CreateUserDataAsync(request.Id, request.Nickname);

        //if (errorCode != ErrorCode.None)
        //{
        //    response.Result = errorCode;
        //    return response;
        //}

        //response.UserGameData = userData;
        return response;
    }
}
