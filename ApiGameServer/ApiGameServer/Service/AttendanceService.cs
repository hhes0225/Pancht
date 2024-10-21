using ApiGameServer.Models.DTO;
using ApiGameServer.Repository;
using ApiGameServer.Service.Interfaces;

namespace ApiGameServer.Service;

public class AttendanceService:IAttendanceService
{
    ILogger<AttendanceService> _logger;
    readonly IPanchtDb _panchtDb;

    public AttendanceService(ILogger<AttendanceService> logger, IPanchtDb userDataDb)
    {
        _logger = logger;
        _panchtDb = userDataDb;
    }

    //출석체크 조회: 며칠째 출석했는지 조회
    public async Task<AttendanceResponse> GetAttendanceAsync(AttendanceRequest request)
    {
        var attendanceResponse = new AttendanceResponse();

        var result = await _panchtDb.GetAttendanceDataAsync(request.Id);

        if(result.Item1 != ErrorCode.None)
        {
            _logger.LogError($"GetAttendanceAsync Error: {result.Item1}");
            attendanceResponse.Result = result.Item1;
            return attendanceResponse;
        }

        attendanceResponse.AttendanceCount = result.Item2.attendance_count;

        return attendanceResponse;
    }

    //출석체크 갱신: 오전 5시 기준으로 출석체크 날짜 변경
    public async Task<AttendanceResponse> UpdateAttendanceAsync(AttendanceRequest request)
    {
        var attendanceResponse = new AttendanceResponse();
        var timeStandard = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 5, 0, 0);

        var result = await _panchtDb.GetAttendanceDataAsync(request.Id);

        if (result.Item1 != ErrorCode.None)
        {
            _logger.LogError($"GetAttendanceAsync Error: {result.Item1}");
            attendanceResponse.Result = result.Item1;
            return attendanceResponse;
        }

        //현재 시간 오전 5시 이전이라면 어제의 오전 5시 구함
        if(DateTime.Now < timeStandard)
        {
            timeStandard = timeStandard.AddDays(-1);
        }

        if(result.Item2.last_attendance_date> timeStandard)
        {
            attendanceResponse.Result = ErrorCode.AttendanceAlreadyDone;
        }

        result.Item2.attendance_count++;
        result.Item2.last_attendance_date = DateTime.Now;

        var updateResult = await _panchtDb.UpdateAttendanceDataAsync(result.Item2);

        if (updateResult != ErrorCode.None)
        {
            _logger.LogError($"UpdateAttendanceAsync Error: {updateResult}");
            attendanceResponse.Result = updateResult;
            return attendanceResponse;
        }



        return attendanceResponse;
    }
}
