using ApiGameServer.Models.DAO;
using System.ComponentModel.DataAnnotations;

namespace ApiGameServer.Models.DTO;

public class CharacterListRequest
{
    [Required]
    public string Id { get; set; } = string.Empty;
}

public class CharacterListResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
    public List<UserCharacterData> UserCharacterList { get; set; } = new List<UserCharacterData>();
}
