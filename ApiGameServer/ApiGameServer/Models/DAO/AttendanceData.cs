namespace ApiGameServer.Models.DAO;

public class AttendanceData
{
    public Int64 uid { get; set; }
    public DateTime? last_attendance_date { get; set; }
    public int attendance_count { get; set; }
}
