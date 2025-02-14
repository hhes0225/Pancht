namespace ApiMatchingServer.Model.DAO;

public class UserMatchInfo
{
    public string Id { get; set; } = string.Empty;
    public int TierScore { get; set; } = 0;
    public int WinStreak { get; set; } = 0;

    public UserMatchInfo(string id, int tierScore, int winStreak)
    {
        Id = id;
        TierScore = tierScore;
        WinStreak = winStreak;
    }

}
