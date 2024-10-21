using ApiGameServer.Models.DTO;
using ApiGameServer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiGameServer.Controllers;

[ApiController]
[Route("[controller]")]
public class UpdateAttendanceController : ControllerBase
{
    ILogger<UpdateAttendanceController> _logger;
    IAttendanceService _service;

    public UpdateAttendanceController(ILogger<UpdateAttendanceController> logger, IAttendanceService attendanceService)
    {
        _logger = logger;
        _service = attendanceService;
    }

    [HttpPost]
    public async Task<AttendanceResponse> GetAttendance([FromBody] AttendanceRequest request)
    {
        AttendanceResponse response = new AttendanceResponse();

        //AttendanceService를 통해 출석 요청
        response = await _service.UpdateAttendanceAsync(request);

        return response;
    }



}
