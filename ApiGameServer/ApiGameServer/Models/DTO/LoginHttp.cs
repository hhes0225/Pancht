using System.ComponentModel.DataAnnotations;

namespace ApiGameServer.Models.DTO;

public class LoginRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "EMAIL CANNOT BE EMPTY")]
    [StringLength(50, ErrorMessage = "EMAIL IS TOO LONG")]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
    public string Id { get; set; } = String.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "AUTHTOKEN CANNOT BE EMPTY")]
    public string AuthToken { get; set; } = String.Empty;
}

public class LoginResponse
{
    [Required]
    public ErrorCode Result { get; set; } = ErrorCode.None;
}


