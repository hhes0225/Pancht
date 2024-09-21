using ApiGameServer.Models.DTO;
using ApiGameServer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiGameServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : Controller
{
    ILogger<ProfileController> _logger;
    IProfileService _service;

    public ProfileController(ILogger<ProfileController> logger, IProfileService profileService)
    {
        _logger = logger;
        _service = profileService;
    }

    [HttpPost]
    public async Task<ProfileResponse> GetProfile([FromBody] ProfileRequest request)
    {
        ProfileResponse response = new ProfileResponse();

        //ProfileService를 통해 프로필 요청
        response = await _service.GetProfileAsync(request);

        return response;
    }

}
