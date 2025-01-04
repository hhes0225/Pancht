using Microsoft.AspNetCore.Mvc;

public class CheckMatchingReq
{
    public string UserID { get; set; }
}


public class CheckMatchingRes
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
    public string ServerAddress { get; set; } = "";
    public int RoomNumber { get; set; } = 0;
}
