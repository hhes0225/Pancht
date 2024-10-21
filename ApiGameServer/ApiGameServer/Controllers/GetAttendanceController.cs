using ApiGameServer.Models.DTO;
using ApiGameServer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiGameServer.Controllers;

[ApiController]
[Route("[controller]")]
public class GetAttendanceController:ControllerBase
{
    ILogger<GetAttendanceController> _logger;
    IAttendanceService _service;

    public GetAttendanceController(ILogger<GetAttendanceController> logger, IAttendanceService attendanceService)
    {
        _logger = logger;
        _service = attendanceService;
    }

    [HttpPost]
    public async Task<AttendanceResponse> GetAttendance([FromBody] AttendanceRequest request)
    {
        AttendanceResponse response = new AttendanceResponse();

        //AttendanceService를 통해 출석 요청
        response = await _service.GetAttendanceAsync(request);

        return response;
    }



}
