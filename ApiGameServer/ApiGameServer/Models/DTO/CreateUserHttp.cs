using ApiGameServer.Models.DAO;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace ApiGameServer.Models.DTO;

public class CreateUserRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "EMAIL CANNOT BE EMPTY")]
    [StringLength(50, ErrorMessage = "EMAIL IS TOO LONG")]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
    public string Id { get; set;} = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "NICKNAME CANNOT BE EMPTY")]
    [StringLength(20, ErrorMessage = "NICKNAME IS TOO LONG")]
    public string Nickname { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "AUTHTOKEN CANNOT BE EMPTY")]
    public string AuthToken { get; set; } = String.Empty;
}

public class CreateUserResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
    public UserData UserGameData { get; set; } = new UserData();
}
