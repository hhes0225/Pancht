namespace ApiGameServer.Models.DAO;

//추후 스키마 바뀔 경우 그에 맞게 변경할 것
public class UserData
{
    public Int64 uid { get; set; }
    public string id { get; set; }
    public string nickname { get; set; }
    public DateTime? create_date { get; set; }
    public DateTime? recent_login_date { get; set; }
}
