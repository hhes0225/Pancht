namespace ApiGameServer.Models.DAO;

public class UserCharacterData
{
    public Int64 user_id { get; set; }
    public string? character_id { get; set; }
    public DateTime? collected_date { get; set; }
}
