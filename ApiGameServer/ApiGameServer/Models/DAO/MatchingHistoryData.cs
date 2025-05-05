namespace ApiGameServer.Models.DAO;

public enum GameResult
{
    Win = 0,
    Lose = 1,
    Draw = 2,
    None = 3
}

public class MatchingHistoryData
{
    public Int64 id { get; set; }
    public string user_id { get; set; }
    public DateTime? match_date { get; set; }
    public GameResult result { get; set; }
    public int tier_score { get; set; }
}
