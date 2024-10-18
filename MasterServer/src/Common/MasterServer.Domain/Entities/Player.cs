using MasterServer.Domain.Entities.BattlePasses;
using MasterServer.Domain.Entities.Economy;
using MasterServer.Domain.Entities.GameSave;
using MasterServer.Domain.Entities.Leaderboard;
using MasterServer.Domain.Entities.Leagues;
using MasterServer.Domain.Entities.Mails;
using MasterServer.Domain.Entities.Socials;
using MasterServer.Domain.Enums;
using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MasterServer.Domain.Entities
{
    public class Player : BaseAuditableEntity
    {
        [Key]
        public long Id { get; set; }

        public string UserName { get; set; }

        public string DeviceId { get; set; }

        public string GoogleId { get; set; }

        public string FacebookId { get; set; }
        [JsonIgnore]
        public string HashedPassword { get; set; }

        [MaxLength(32)]
        public string NickName { get; set; }


        [MaxLength(32)]
        public string Email { get; set; }

        public bool Initialized { get; set; }

        public string ProfileSyncVersion { get; set; }

        public PlayerLockKind LockKind { get; set; } = PlayerLockKind.None;

        public Instant? LockToDate { get; set; }

        public string LockReason { get; set; }

        public string CountryCode { get; set; }

        public string GameVersion { get; set; }

        public Instant? LatestOnlineAt { get; set; }

        public Instant? LatestOfflineAt { get; set; }

        public int SelectedAvatarId { get; set; }

        public PlayerRole Role { get; set; }

        public PlayerFeatureStatus FeatureStatus { get; set; }

        public List<PlayerData> PlayerDatas { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }


        #region Economy
        public List<PlayerBalance> PlayerBalances { get; set; } = new List<PlayerBalance>();
        public List<PlayerInventory> PlayerInventorys { get; set; } = new List<PlayerInventory>();
        public List<PlayerShopRealMoneyPurchaseReceipt> RealMoneyPurchaseReceipts { get; set; } = new List<PlayerShopRealMoneyPurchaseReceipt>();
        public List<PlayerShopVirtualPurchaseReceipt> VirtualPurchaseReceipts { get; set; } = new List<PlayerShopVirtualPurchaseReceipt>();
        #endregion

        public List<GameSession> Sessions { get; set; } = new List<GameSession>();


        [InverseProperty(nameof(PlayerEdge.SrcPlayer))]
        public List<PlayerEdge> PlayerEdgeAsSrcPlayer { get; set; } = new List<PlayerEdge>();

        [InverseProperty(nameof(PlayerEdge.DstPlayer))]
        public List<PlayerEdge> PlayerEdgeAsDstPlayer { get; set; } = new List<PlayerEdge>();

        public List<ConversationParticipant> ConversationParticipants { get; set; } = new List<ConversationParticipant>();

        public List<PlayerMail> MailBox { get; set; } = new List<PlayerMail>();


        public List<BattlePassSeasonParticipant> BattlePassParticipants { get; set; } = new List<BattlePassSeasonParticipant>();

        public List<LeaderboardVersionParticipant> LeaderboardVersionParticipants { get; set; } = new List<LeaderboardVersionParticipant>();

        public List<PlayerFile> PlayerFiles { get; set; } = new List<PlayerFile>();

    }
}
