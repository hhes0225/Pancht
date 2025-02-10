using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiMatchingServer.Models.DTO;

public class MatchingRequest
{
	public string UserID { get; set; }=string.Empty;
	public int TierScore { get; set; }=0;
}

public class MatchResponse
{
    [Required]
    public ErrorCode Result { get; set; } = ErrorCode.None;
}