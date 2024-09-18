using System.ComponentModel.DataAnnotations;

namespace ApiAccountServer.Models.DTO;

public class VerifyTokenRequest
{
    [Required]
    public string Id { get; set; }= string.Empty;
    [Required]
    public string AuthToken { get; set; } = string.Empty;
}

public class VerifyTokenResponse
{
    [Required]
    public ErrorCode Result { get; set; } = ErrorCode.None;
}
