using ApiGameServer.Models.DTO;

namespace ApiGameServer.Service.Interfaces;

public interface IAttendanceService
{
    public Task<AttendanceResponse> GetAttendanceAsync(AttendanceRequest request);
    public Task<AttendanceResponse> UpdateAttendanceAsync(AttendanceRequest request);
}
