using Microsoft.AspNetCore.Mvc;

public class MatchingRequest
{
	public string UserID { get; set; }
}

public class MatchResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;
}