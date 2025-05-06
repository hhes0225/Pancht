using ApiGameServer.Models.DAO;
using System.ComponentModel.DataAnnotations;

namespace ApiGameServer.Models.DTO;

public class MatchingRequestFromClient
{
    [Required]
    public string Id { get; set; } = string.Empty;
}
public class MatchingRequest
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public int TierScore { get; set; } = 0;
    [Required]
    public GameResult LastGameResult { get; set; } = GameResult.None;
}

public class MatchingResponse
{
    public ErrorCode Result { get; set; }
}

public class CancelMatchingRequest
{
    [Required]
    public string Id { get; set; } = string.Empty;
}

public class CancelMatchingResponse
{
    public ErrorCode Result { get; set; }
}

public class CheckMatchingRequest
{
    [Required]
    public string Id { get; set; } = string.Empty;
}

public class CheckMatchingResponse
{
    public ErrorCode Result { get; set; }
    public string? SocketServerAddress { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
    public int RoomNumber { get; set; } = 0;
}