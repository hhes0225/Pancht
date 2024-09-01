namespace ApiAccountServer.Models.DAO;

public class AccountDb
{
    public Int64 uid { get; set; }
    public string id { get; set; }
    public string pw { get; set; }
    public string create_date { get; set; }
    public string recent_login_date { get; set; }
}
