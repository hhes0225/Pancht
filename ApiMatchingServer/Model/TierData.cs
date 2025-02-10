using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiMatchingServer.Model;

public enum TierType
{
    None,
    Bronze1,
    Bronze2,
    Bronze3,
    Silver1,
    Silver2,
    Silver3,
    Gold1,
    Gold2,
    Gold3,
}

public class Tier
{
    public TierType Id { get; set; }
    public int MinScore { get; set; }
    public int MaxScore { get; set; }

    public Tier(TierType id, int minScore, int maxScore)
    {
        Id = id;
        MinScore = minScore;
        MaxScore = maxScore;
    }
}

public static class TierInfo
{
    public static readonly List<Tier> TierList = new List<Tier>
    {
        new Tier(TierType.None, Int32.MinValue, 0),
        new Tier(TierType.Bronze1, 1, 100),//0~100?
        new Tier(TierType.Bronze2, 101, 200),
        new Tier(TierType.Bronze3, 201, 350),
        new Tier(TierType.Silver1, 351, 450),
        new Tier(TierType.Silver2, 451, 550),
        new Tier(TierType.Silver3, 551, 700),
        new Tier(TierType.Gold1, 701, 800),
        new Tier(TierType.Gold2, 801, 900),
        new Tier(TierType.Gold3, 901, Int32.MaxValue)//901점 이상은 최대값으로 처리
    };

    public static Tier GetTierByScore(int score)
    {
        return TierList.FirstOrDefault(tier => tier.MinScore <= score && score <= tier.MaxScore);
    }
}
