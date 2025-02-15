﻿using System.ComponentModel.DataAnnotations;

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
}

public class MatchingResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
}

public class CancelMatchingRequest
{
    [Required]
    public string Id { get; set; } = string.Empty;
}

public class CancelMatchingResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
}

public class CheckMatchingRequest
{
    [Required]
    public string Id { get; set; } = string.Empty;
}

public class CheckMatchingResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
    public string? SocketServerAddress { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
    public int RoomNumber { get; set; } = 0;
}