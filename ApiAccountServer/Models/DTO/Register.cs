using System.ComponentModel.DataAnnotations;

namespace ApiAccountServer.Models.DTO;

public class RegisterRequest
{
    [Required]
    [EmailAddress(ErrorMessage ="EMAIL IS NOT VALID")]
    [MinLength(1, ErrorMessage = "EMAIL CANNOT BE EMPTY")]
    [StringLength(50, ErrorMessage = "EMAIL MUST BE LESS THAN 50 CHARACTERS")]

    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage ="PASSWORD CANNOT BE EMPTY")]
    [StringLength(20, ErrorMessage = "PASSWORD MUST BE LESS THAN 20 CHARACTERS")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(20, ErrorMessage = "PASSWORD MUST BE LESS THAN 20 CHARACTERS")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class RegisterResponse
{
    [Required]
    public ErrorCode Result { get; set; }= ErrorCode.None;
}
