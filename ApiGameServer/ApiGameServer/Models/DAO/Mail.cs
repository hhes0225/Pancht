namespace ApiGameServer.Models.DAO;

public class Mail
{
    public Int64 mail_id { get; set; }
    public Int64 user_id { get; set; }
    public string title { get; set; }
    public string content { get; set; }
    public DateTime? send_date { get; set; }
    public bool is_read { get; set; }
}
