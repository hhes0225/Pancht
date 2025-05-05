using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiMatchingServer.Models.DTO;

public enum GameResult
{
    Win = 0,
    Lose = 1,
    Draw = 2,
    None = 3
}

public class MatchingRequest
{
	public string UserID { get; set; }=string.Empty;
	public int TierScore { get; set; }=0;
    public GameResult LastGameResult {  get; set; }=GameResult.None;
}

public class MatchingResponse
{
    [Required]
    public ErrorCode Result { get; set; } = ErrorCode.None;
}