using System.ComponentModel.DataAnnotations;

namespace ApiGameServer.Models.DTO;

public class AttendanceRequest
{
    [Required]
    public string Id { get; set;} = string.Empty;
}

public class  AttendanceResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
    public int AttendanceCount { get; set; }
}
