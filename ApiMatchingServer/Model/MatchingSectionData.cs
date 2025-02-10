using System.Collections.Generic;
using System.Linq;

namespace ApiMatchingServer.Model;


public enum MatchingSectionType
{
    Section1,
    Section2,
    Section3,
    Section4,
    Section5
}

public class MatchingSection
{
    public MatchingSectionType Id { get; set; }
    public TierType MinTier { get; set; }
    public TierType MaxTier { get; set; }

    public MatchingSection(MatchingSectionType id, TierType minTier, TierType maxTier)
    {
        Id = id;
        MinTier = minTier;
        MaxTier = maxTier;
    }
}

public static class MatchingSectionInfo {
    public static readonly List<MatchingSection> MatchingSectionList = new List<MatchingSection>
    {
        new MatchingSection(MatchingSectionType.Section1, TierType.Bronze1, TierType.Silver1),//0~100?
        new MatchingSection(MatchingSectionType.Section2, TierType.Bronze3, TierType.Silver3),
        new MatchingSection(MatchingSectionType.Section3, TierType.Silver1, TierType.Gold1),
        new MatchingSection(MatchingSectionType.Section4, TierType.Silver3, TierType.Gold3),
        new MatchingSection(MatchingSectionType.Section5, TierType.Gold1, TierType.Gold3)//451점 이상은 최대값으로 처리
    };

    public static MatchingSection GetMatchingSectionByTier(TierType tier)
    {
        return MatchingSectionList.FirstOrDefault(section => section.MinTier <= tier && tier <= section.MaxTier);
    }
}