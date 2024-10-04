using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Net.WebSockets;

namespace MasterServer.Domain.Enums
{
    public enum GameLeagueRank
    {
        Unranked,
        Wood,
        Wood3,
        Wood2,
        Wood1,
        Bronze,
        Bronze3,
        Bronze2,
        Bronze1,
        Silver,
        Silver3,
        Silver2,
        Silver1,
        Gold,
        Gold3,
        Gold2,
        Gold1,

        Master,
        Master3,
        Master2,
        Master1,
        Champion,
        Champion3,
        Champion2,
        Champion1,
        Legend,
        Legend3,
        Legend2,
        Legend1,

    }

    public enum GameLeagueTier
    {
        Unranked,
        Wood,
        Bronze,
        Silver,
        Gold,
        Master,
        Champion,
        Legend
    }


    public static class LeagueHelper
    {
        public static GameLeagueTier GetTier(GameLeagueRank rank)
        {
            if (rank == GameLeagueRank.Unranked)
            {
                return GameLeagueTier.Unranked;
            }
            else if (rank == GameLeagueRank.Bronze || rank == GameLeagueRank.Bronze3 || rank == GameLeagueRank.Bronze2 || rank == GameLeagueRank.Bronze1)
            {
                return GameLeagueTier.Bronze;
            }
            else if (rank == GameLeagueRank.Silver || rank == GameLeagueRank.Silver3 || rank == GameLeagueRank.Silver2 || rank == GameLeagueRank.Silver1)
            {
                return GameLeagueTier.Silver;
            }
            else if (rank == GameLeagueRank.Gold || rank == GameLeagueRank.Gold3 || rank == GameLeagueRank.Gold2 || rank == GameLeagueRank.Gold1)
            {
                return GameLeagueTier.Gold;
            }
            else if (rank == GameLeagueRank.Wood || rank == GameLeagueRank.Wood3 || rank == GameLeagueRank.Wood2 || rank == GameLeagueRank.Wood1)
            {
                return GameLeagueTier.Wood;
            }
            else if (rank == GameLeagueRank.Master || rank == GameLeagueRank.Master3 || rank == GameLeagueRank.Master2 || rank == GameLeagueRank.Master1)
            {
                return GameLeagueTier.Master;
            }
            else if (rank == GameLeagueRank.Champion || rank == GameLeagueRank.Champion3 || rank == GameLeagueRank.Champion2 ||
                     rank == GameLeagueRank.Champion1)
            {
                return GameLeagueTier.Champion;
            }

            return GameLeagueTier.Legend;
        }

        public static List<GameLeagueRank> GetTierRank(GameLeagueTier tier)
        {
            switch (tier)
            {
                case GameLeagueTier.Legend:
                    return new List<GameLeagueRank>()
                        { GameLeagueRank.Legend, GameLeagueRank.Legend3,GameLeagueRank.Legend2, GameLeagueRank.Legend1,};
                case GameLeagueTier.Champion:
                    return new List<GameLeagueRank>()
                        { GameLeagueRank.Champion, GameLeagueRank.Champion3,GameLeagueRank.Champion2, GameLeagueRank.Champion1,};
                case GameLeagueTier.Master:
                    return new List<GameLeagueRank>()
                        {GameLeagueRank.Master,GameLeagueRank.Master3, GameLeagueRank.Master2, GameLeagueRank.Master1, };
                case GameLeagueTier.Wood:
                    return new List<GameLeagueRank>()
                        {GameLeagueRank.Wood,GameLeagueRank.Wood3,GameLeagueRank.Wood2, GameLeagueRank.Wood1, };
                case GameLeagueTier.Gold:
                    return new List<GameLeagueRank>()
                        { GameLeagueRank.Gold, GameLeagueRank.Gold3,GameLeagueRank.Gold2, GameLeagueRank.Gold1,};
                case GameLeagueTier.Silver:
                    return new List<GameLeagueRank>()
                        {GameLeagueRank.Silver, GameLeagueRank.Silver3,GameLeagueRank.Silver2, GameLeagueRank.Silver1, };
                case GameLeagueTier.Bronze:
                    return new List<GameLeagueRank>()
                        {GameLeagueRank.Bronze, GameLeagueRank.Bronze3,GameLeagueRank.Bronze2, GameLeagueRank.Bronze1, };
                case GameLeagueTier.Unranked:
                    return new List<GameLeagueRank>()
                        {GameLeagueRank.Unranked};
            }

            return null;
        }

        public static List<GameLeagueRank> GetMatchmakingRankForTier(GameLeagueTier tier)
        {
            var lst = new List<GameLeagueTier>() { tier };
            if (tier > GameLeagueTier.Bronze)
            {
                lst.Add(tier - 1);
            }
            if (tier < GameLeagueTier.Legend)
            {
                lst.Add(tier + 1);
            }
            var ranks = lst.SelectMany(x => GetTierRank(x)).ToList();
            return ranks;
        }
    }
}

