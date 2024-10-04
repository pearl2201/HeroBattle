using MasterServer.Domain.Enums;
using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Game
{
    public class GameMatch : BaseAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        public long AttackerId { get; set; }
        [ForeignKey("AttackerId")]
        public Player Attacker { get; set; }

        public float CompletePercentage { get; set; }

        public GameMatchStatus Status { get; set; }

        public int AttackerStar { get; set; }

        public int DefenderStar { get; set; }

        public Instant? CompletedAt { get; set; }

        public Instant? QuittedAt { get; set; }

        public Instant? DefenderRewardsClaimedAt { get; set; }

        public Instant? RevengedAt { get; set; }

        public byte[] ReplayData { get; set; }

        public int AttackerPower { get; set; }

        public int DefenserPower { get; set; }

        public int AttackerMmr { get; set; }

        public int DefenserMmr { get; set; }

        public GameMatchDefenderFoundType DefenderFoundType { get; set; }
        [MaxLength(36)]
        public string RevegenMatchId { get; set; }

        public void SetGameComplete()
        {
           
        }
    }

    public enum GameMatchDefenderFoundType
    {
        Matchmaking,
        Specific,
        Reverge,
        Replay,
        FillDefenseMatch,
    }

    public class Vector2f
    {
        public float X { get; set; }

        public float Y { get; set; }

        public Vector2f()
        {
            X = 0;
            Y = 0;
        }

        public Vector2f(float x, float y)
        {
            X = x;
            Y = y;
        }

    }

}


