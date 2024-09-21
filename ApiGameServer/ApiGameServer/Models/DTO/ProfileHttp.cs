using ApiGameServer.Models.DAO;
using System.ComponentModel.DataAnnotations;

namespace ApiGameServer.Models.DTO;

public class ProfileRequest
{
    [Required]
    public string Id { get; set; } = string.Empty;
}

public class ProfileResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
    public UserData UserGameData { get; set; } = new UserData();
}
