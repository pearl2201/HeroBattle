using NodaTime;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.GameSave
{
    public class PlayerBattleProfile
    {
        public int SuccessfulAttack { get; set; }

        public int SuccessfulDefense { get; set; }

        public int StreakSuccessfulAttack { get; set; }

        public int StreakFailureAttack { get; set; }

        public bool IsInStreakSuccessfulAttack { get; set; }

        public bool IsInStreakFailureAttack { get; set; }

        public int MatchmakingStreak { get; set; }
    }
}
