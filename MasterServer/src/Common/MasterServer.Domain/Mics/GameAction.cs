namespace MasterServer.Domain.Mics
{
    public static class GameAction
    {

        public const string Create = nameof(Create);
        public const string Battle = nameof(Battle);
        public const string Terminate = nameof(Terminate);
        public const string Receive = nameof(Receive);
        public const string Upgrade = nameof(Upgrade);
        public const string Update = nameof(Update);
        public const string Map = nameof(Map);
        public const string Send = nameof(Send);
        public const string Buy = nameof(Buy);
        public const string Deposit = nameof(Deposit);
        public const string Withdraw = nameof(Withdraw);
        public const string Sell = nameof(Sell);
        public const string Return = nameof(Return);
        public const string LevelUp = nameof(LevelUp);
        public const string Breed = nameof(Breed);
        public const string Gear = nameof(Gear);
        public const string Login = nameof(Login);
        public const string Exchange = nameof(Exchange);
    }


    public static class GameSubAction
    {
        public const string Login = nameof(Login);
        public const string NewSession = nameof(NewSession);
        public const string Register = nameof(Register);
        public const string CreateOtp = nameof(CreateOtp);
        public const string ChangePassword = nameof(ChangePassword);
        public const string ForgotPassword = nameof(ForgotPassword);
        public const string LockAccount = nameof(LockAccount);
        public const string CreateAccount = nameof(CreateAccount);
        public const string CreateLeagueProfile = nameof(CreateLeagueProfile);
        public const string UnlockAccount = nameof(UnlockAccount);
        public const string LoginByOtp = nameof(LoginByOtp);
        public const string LoginByPassword = nameof(LoginByPassword);
        public const string ResetPassword = nameof(ResetPassword);
        public const string SyncProfile = nameof(SyncProfile);
        public const string RefreshToken = nameof(RefreshToken);
        public const string CreateProfile = nameof(CreateProfile);
        public const string UpdateTeamcomp = nameof(UpdateTeamcomp);
        public const string UpdateGameProfile = nameof(UpdateGameProfile);
        public const string SyncHero = nameof(SyncHero);
        public const string LevelUpHero = nameof(LevelUpHero);
        public const string StarUpHero = nameof(StarUpHero);
        public const string RarityUpHero = nameof(RarityUpHero);
        public const string UpgradeHeroBodyPart = nameof(UpgradeHeroBodyPart);
        public const string ResetHeroLevel = nameof(ResetHeroLevel);
        public const string StartPve = nameof(StartPve);
        public const string EndPve = nameof(EndPve);
        public const string DiscardPve = nameof(DiscardPve);
        public const string StartPvp = nameof(StartPvp);
        public const string EndPvp = nameof(EndPvp);
        public const string ClaimDefenderRewards = nameof(ClaimDefenderRewards);
        public const string DiscardPvp = nameof(DiscardPvp);
        public const string JoinPvp = nameof(JoinPvp);
        public const string DepositKab = nameof(DepositKab);
        public const string WithdrawKab = nameof(WithdrawKab);
        public const string CreateScholarship = nameof(CreateScholarship);
        public const string StopScholarship = nameof(StopScholarship);
        public const string UpdateScholarship = nameof(UpdateScholarship);
        public const string PayoutScholarship = nameof(PayoutScholarship);
        public const string ClaimScholarship = nameof(ClaimScholarship);
        public const string TransferAssetToSubAccount = nameof(TransferAssetToSubAccount);
        public const string TournamentTransferReward = nameof(TournamentTransferReward);
        public const string AdminTransferReward = nameof(AdminTransferReward);
        public const string ExchangeWildcard = nameof(ExchangeWildcard);
        public const string ExchangeKab2Okg = nameof(ExchangeKab2Okg);
        public const string ExchangeOkg2Kab = nameof(ExchangeOkg2Kab);
        public const string OrderItems = nameof(OrderItems);


        public const string PlayerGotMail = nameof(PlayerGotMail);
        public const string ClaimMailReward = nameof(ClaimMailReward);
        public const string PurchaseBuilding = nameof(PurchaseBuilding);
        public const string UpdateBuilding = nameof(UpdateBuilding);
        public const string UpdateFtueStep = nameof(UpdateFtueStep);
        public const string UpdateTeam = nameof(UpdateTeam);
        public const string UpdateInventory = nameof(UpdateInventory);

        public const string PurchaseIap = nameof(PurchaseIap);
        public const string PurchaseShop = nameof(PurchaseShop);

        public const string UpdateLeagueStar = nameof(UpdateLeagueStar);

        public const string UpdateLeagueRank = nameof(UpdateLeagueRank);
        public const string ReceiveLeagueRankReward = nameof(ReceiveLeagueRankReward);
        public const string AssignPlayerToLeagueLeaderboard = nameof(AssignPlayerToLeagueLeaderboard);

        public const string UnlockBattlePass = nameof(UnlockBattlePass);
        public const string ClaimBattlePassReward = nameof(ClaimBattlePassReward);
    }

    public static class GameSubject
    {
        public const string Bot = nameof(Bot);
        public const string User = nameof(User);
        public const string Battle = nameof(Battle);
        public const string Hero = nameof(Hero);
        public const string Building = nameof(Building);
        public const string AutoResult = nameof(AutoResult);
        public const string Shop = nameof(Shop);
        public const string Mail = nameof(Mail);
        public const string InventoryItem = nameof(InventoryItem);
        public const string Breed = nameof(Breed);
        public const string Scholarship = nameof(Scholarship);
        public const string League = nameof(League);
        public const string BattlePass = nameof(BattlePass);
    }

    public static class GameActionLogParamaterKey
    {
        public const string Currencies = nameof(Currencies);
        public const string Buildings = nameof(Buildings);
        public const string Heroes = nameof(Heroes);
        public const string Modules = nameof(Modules);
        public const string Bridges = nameof(Bridges);
        public const string MapType = nameof(MapType);
        public const string TreePositions = nameof(TreePositions);
        public const string FtueStep = nameof(FtueStep);
        public const string IsCompletedFtue = nameof(IsCompletedFtue);
        public const string FirebasePlayerId = nameof(FirebasePlayerId);
        public const string GameVersion = nameof(GameVersion);

        public const string SubjectId = nameof(SubjectId);
        public const string SubjectIds = nameof(SubjectIds);
        public const string InventoryChanged = nameof(InventoryChanged);

        public const string LeagueRank = nameof(LeagueRank);

        public const string AddExp = nameof(AddExp);
        public const string SubExp = nameof(SubExp);
        public const string Type = nameof(Type);
        public const string Id = nameof(Id);
        public const string OrderId = nameof(OrderId);
        public const string WildCards = nameof(WildCards);
        public const string ConsumeItems = nameof(ConsumeItems);
        public const string AddItems = nameof(AddItems);
        public const string AddWildCards = nameof(AddWildCards);
        public const string SubWhiteCards = nameof(SubWhiteCards);
        public const string AddHeroLevelTo = nameof(AddHeroLevelTo);
        public const string ResetHeroLevelFrom = nameof(ResetHeroLevelFrom);
        public const string BodyPart = nameof(BodyPart);
        public const string AddBodyPartLevelTo = nameof(AddBodyPartLevelTo);
        public const string Stage = nameof(Stage);
        public const string GameId = nameof(GameId);
        public const string Team = nameof(Team);
        public const string MatchStatus = nameof(MatchStatus);
        public const string PvpResult = nameof(PvpResult);
        public const string GameStar = nameof(GameStar);
        public const string RemainEnergy = nameof(RemainEnergy);
        public const string Teamcomp = nameof(Teamcomp);
        public const string Avatar = nameof(Avatar);
        public const string AdminOidcId = nameof(AdminOidcId);
        public const string NickName = nameof(NickName);
        public const string CastleLevel = nameof(CastleLevel);
        public const string LockReason = nameof(LockReason);
        public const string LockToDate = nameof(LockToDate);
        public const string LockKind = nameof(LockKind);
        public const string Email = nameof(Email);
        public const string IpAddress = nameof(IpAddress);
        public const string UserAgent = nameof(UserAgent);
        public const string UaPlatform = nameof(UaPlatform);
        public const string UaMobile = nameof(UaMobile);
        public const string IpCountry = nameof(IpCountry);
        public const string UaVersion = nameof(UaVersion);
        public const string Count = nameof(Count);
        public const string TimeAction = nameof(TimeAction);
        public const string MkpScholarshipId = nameof(MkpScholarshipId);
        public const string AddHeroes = nameof(AddHeroes);
        public const string RemoveSubAccHeroes = nameof(RemoveSubAccHeroes);

        public const string TransactionId = nameof(TransactionId);
        public const string TournamentId = nameof(TournamentId);
        public const string SourceAction = nameof(SourceAction);
        public const string AdminOidc = nameof(AdminOidc);
        public const string UtmSource = nameof(UtmSource);
        public const string UtmCampaign = nameof(UtmCampaign);
        public const string UtmMedium = nameof(UtmMedium);
        public const string UtmContent = nameof(UtmContent);
        public const string UnityPlatform = nameof(UnityPlatform);


        public const string RemoveAssets = nameof(RemoveAssets);
        public const string AttackerRewards = nameof(AttackerRewards);
        public const string DefenderRewards = nameof(DefenderRewards);
        public const string ResultUpdateAssets = nameof(ResultUpdateAssets);
        public const string Receipt = nameof(Receipt);
        public const string ShopPackageId = nameof(ShopPackageId);

        public const string FromValue = nameof(FromValue);
        public const string ToValue = nameof(ToValue);
        public const string AttackerId = nameof(AttackerId);
        public const string DefenderId = nameof(DefenderId);
        public const string AttackerFormation = nameof(AttackerFormation);
        public const string DefenderFormation = nameof(DefenderFormation);
        public const string AttackerStarReceive = nameof(AttackerStarReceive);
        public const string DefenderStarReceive = nameof(DefenderStarReceive);
        public const string AttackerStarAfter = nameof(AttackerStarAfter);
        public const string DefenderStarAfter = nameof(DefenderStarAfter);
        public const string AttackerLeaguePointAfter = nameof(AttackerLeaguePointAfter);
        public const string Reason = nameof(Reason);
        public const string ReasonRef = nameof(ReasonRef);

        public const string LeagueStarReceive = nameof(LeagueStarReceive);
        public const string LeagueStarAfter = nameof(LeagueStarAfter);
        public const string LeagueSeasonId = nameof(LeagueSeasonId);
        public const string LeagueUnlockRank = nameof(LeagueUnlockRank);
        public const string LeagueRanks = nameof(LeagueRanks);
        public const string LeagueCreateProfile = nameof(LeagueCreateProfile);

        public const string BattlePassEventId = nameof(BattlePassEventId);
        public const string BattlePassLevel = nameof(BattlePassLevel);
        public const string IsPremiumReward = nameof(IsPremiumReward);
        public static string PlayerTeamSlot(int slotId) => nameof(PlayerTeamSlot) + "slotId";
    }

    public enum ActionLogSource
    {
        Game,
        Marketplace,
        Admin,
        System
    }

    public static class AdminAction
    {
        public const string Update = nameof(Update);
        public const string Create = nameof(Create);
        public const string Delete = nameof(Delete);
        public const string Deploy = nameof(Deploy);
        public const string Clone = nameof(Clone);
    }

    public static class AdminSubAction
    {

        public const string CreateGameTag = nameof(CreateGameTag);
        public const string UpdateGameTag = nameof(UpdateGameTag);
        public const string DeleteGameTag = nameof(DeleteGameTag);

        public const string AddItemToGameTag = nameof(AddItemToGameTag);
        public const string RemoveItemToGameTag = nameof(RemoveItemToGameTag);

        public const string CreateGameFeature = nameof(CreateGameFeature);
        public const string UpdateGameFeature = nameof(UpdateGameFeature);
        public const string DeleteGameFeature = nameof(DeleteGameFeature);

        public const string AddOverrideFeatureToVersion = nameof(AddOverrideFeatureToVersion);
        public const string RemoveOverrideFeatureToVersion = nameof(RemoveOverrideFeatureToVersion);

        public const string CreateGameAsset = nameof(CreateGameAsset);
        public const string UpdateGameAsset = nameof(UpdateGameAsset);
        public const string DeleteGameAsset = nameof(DeleteGameAsset);
        public const string DeployGameAsset = nameof(DeployGameAsset);
        public const string UpdatePriviewGameAsset = nameof(UpdatePriviewGameAsset);

        public const string CreateKingPassEvent = nameof(CreateKingPassEvent);
        public const string UpdateKingPassEvent = nameof(UpdateKingPassEvent);
        public const string DeleteKingPassEvent = nameof(DeleteKingPassEvent);
        public const string CreateKingPassEventProgress = nameof(CreateKingPassEventProgress);
        public const string UpdateKingPassEventProgress = nameof(UpdateKingPassEventProgress);
        public const string DeleteKingPassEventProgress = nameof(DeleteKingPassEventProgress);
        public const string CreateKingPassEventLteProgress = nameof(CreateKingPassEventLteProgress);
        public const string UpdateKingPassEventLteProgress = nameof(UpdateKingPassEventLteProgress);
        public const string DeleteKingPassEventLteProgress = nameof(DeleteKingPassEventLteProgress);
        public const string DeployKingPassEvent = nameof(DeployKingPassEvent);

        public const string CreateGachaBoxConfig = nameof(CreateGachaBoxConfig);
        public const string UpdateGachaBoxConfig = nameof(UpdateGachaBoxConfig);
        public const string DeleteGachaBoxConfig = nameof(DeleteGachaBoxConfig);
        public const string CreateGachaBoxRatioConfig = nameof(CreateGachaBoxRatioConfig);
        public const string UpdateGachaBoxRatioConfig = nameof(UpdateGachaBoxRatioConfig);
        public const string DeleteGachaBoxRatioConfig = nameof(DeleteGachaBoxRatioConfig);
        public const string DeployGachaBoxConfig = nameof(DeployGachaBoxConfig);
        public const string DeployWeeklySpinConfig = nameof(DeployWeeklySpinConfig);
        public const string DeploySpecialEventGachaConfig = nameof(DeploySpecialEventGachaConfig);

        public const string CreateShopGroup = nameof(CreateShopGroup);
        public const string UpdateShopGroup = nameof(UpdateShopGroup);
        public const string DeleteShopGroup = nameof(DeleteShopGroup);

        public const string CreateShopGroupItem = nameof(CreateShopGroupItem);
        public const string UpdateShopGroupItem = nameof(UpdateShopGroupItem);
        public const string DeleteShopGroupItem = nameof(DeleteShopGroupItem);


        public const string CreateGameLiftConfig = nameof(CreateGameLiftConfig);
        public const string UpdateGameLiftConfig = nameof(UpdateGameLiftConfig);
        public const string DeleteGameLiftConfig = nameof(DeleteGameLiftConfig);

        public const string CreateGameVersion = nameof(CreateGameVersion);
        public const string UpdateGameVersion = nameof(UpdateGameVersion);
        public const string DeleteGameVersion = nameof(DeleteGameVersion);
        public const string ChangeGameVersionPlatformGameLiftConfigId = nameof(ChangeGameVersionPlatformGameLiftConfigId);
        public const string ChangeGameVersionPlatformStatus = nameof(ChangeGameVersionPlatformStatus);


        public const string CreateGameMail = nameof(CreateGameMail);
        public const string UpdateGameMail = nameof(UpdateGameMail);
        public const string DeleteGameMail = nameof(DeleteGameMail);
        public const string AddGameMailAttachment = nameof(AddGameMailAttachment);
        public const string RemoveGameMailAttachment = nameof(RemoveGameMailAttachment);
        public const string AddGameMailPlayers = nameof(AddGameMailPlayers);
        public const string RemoveGameMailPlayer = nameof(RemoveGameMailPlayer);

        public const string CreateGameMailTemplate = nameof(CreateGameMailTemplate);
        public const string UpdateGameMailTemplate = nameof(UpdateGameMailTemplate);
        public const string DeleteGameMailTemplate = nameof(DeleteGameMailTemplate);

        public const string CreatePromotionCampaign = nameof(CreatePromotionCampaign);
        public const string UpdatePromotionCampaign = nameof(UpdatePromotionCampaign);
        public const string DeletePromotionCampaign = nameof(DeletePromotionCampaign);
        public const string AddPromotionCampaignAttachment = nameof(AddPromotionCampaignAttachment);
        public const string RemovePromotionCampaignAttachment = nameof(RemovePromotionCampaignAttachment);



        public const string CreateLeagueEvent = nameof(CreateLeagueEvent);
        public const string UpdateLeagueEvent = nameof(UpdateLeagueEvent);
        public const string DeleteLeagueEvent = nameof(DeleteLeagueEvent);
        public const string PublishLeagueEvent = nameof(PublishLeagueEvent);
        public const string UpdateLeague = nameof(UpdateLeague);
        public const string CreateMilestone = nameof(CreateMilestone);
        public const string UpdateMilestone = nameof(UpdateMilestone);
        public const string UpdateMilestoneReward = nameof(UpdateMilestoneReward);
        public const string DeleteMilestone = nameof(DeleteMilestone);
        public const string AddLeagueSeasonReward = nameof(AddLeagueSeasonReward);
        public const string RemoveLeagueSeasonReward = nameof(RemoveLeagueSeasonReward);
        public const string AddLeaguePromotionReward = nameof(AddLeaguePromotionReward);
        public const string RemoveLeaguePromotionReward = nameof(RemoveLeaguePromotionReward);
        public const string CreateMilestoneReward = nameof(CreateMilestoneReward);
        public const string RemoveMilestoneReward = nameof(RemoveMilestoneReward);
        public const string CreateLeaguePromotionGroup = nameof(CreateLeaguePromotionGroup);
        public const string UpdateLeaguePromotionGroup = nameof(UpdateLeaguePromotionGroup);
        public const string DeleteLeaguePromotionGroup = nameof(DeleteLeaguePromotionGroup);

        public const string CreateDailyRewardConfigSet = nameof(CreateDailyRewardConfigSet);
        public const string UpdateDailyRewardConfigSet = nameof(UpdateDailyRewardConfigSet);
        public const string DeleteDailyRewardConfigSet = nameof(DeleteDailyRewardConfigSet);
        public const string CreateDailyRewardGift = nameof(CreateDailyRewardGift);
        public const string UpdateDailyRewardGift = nameof(UpdateDailyRewardGift);
        public const string DeleteDailyRewardGift = nameof(DeleteDailyRewardGift);
        public const string DeployDailyRewardConfigSet = nameof(DeployDailyRewardConfigSet);

    }

    public static class AdminActionSubject
    {
        public const string Profile = nameof(Profile);
        public const string GameAsset = nameof(GameAsset);
        public const string GameTag = nameof(GameTag);
        public const string GameFeature = nameof(GameFeature);
        public const string KingPass = nameof(KingPass);
        public const string KingPassProgress = nameof(KingPassProgress);
        public const string KingPassLteProgress = nameof(KingPassLteProgress);
        public const string GachaBoxConfig = nameof(GachaBoxConfig);
        public const string GachaBoxRatioConfig = nameof(GachaBoxRatioConfig);
        public const string ShopGroup = nameof(ShopGroup);
        public const string ShopGroupItem = nameof(ShopGroupItem);
        public const string WeeklySpinConfig = nameof(WeeklySpinConfig);
        public const string SpecialEventGachaConfig = nameof(SpecialEventGachaConfig);
        public const string GameLiftConfig = nameof(GameLiftConfig);
        public const string GameVersion = nameof(GameVersion);
        public const string GameMail = nameof(GameMail);
        public const string GameMailTemplate = nameof(GameMailTemplate);
        public const string PromotionCampaign = nameof(PromotionCampaign);

        public const string LeagueEvent = nameof(LeagueEvent);
        public const string League = nameof(League);
        public const string LeagueMilestone = nameof(LeagueMilestone);
        public const string LeaguePromotionGroup = nameof(LeaguePromotionGroup);

        public const string DailyRewardConfigSet = nameof(DailyRewardConfigSet);
        public const string DailyRewardGift = nameof(DailyRewardGift);

    }


    public static class AdminActionParameterKey
    {
        public const string Id = nameof(Id);
        public const string VersionId = nameof(VersionId);
        public const string Platform = nameof(Platform);
        public const string CloneToId = nameof(CloneToId);
        public const string DeployName = nameof(DeployName);
        public const string ContentRequest = nameof(ContentRequest);
        public const string GameConfigVersion = nameof(GameConfigVersion);
    }

    public static class SystemSubAction
    {
        public const string UpdateCCU = nameof(UpdateCCU);
        public const string RefreshBetaLeaderboard = nameof(RefreshBetaLeaderboard);
        public const string UpdatePancakeExchange = nameof(UpdatePancakeExchange);
        public const string CalculateStageKabReserverJob = nameof(CalculateStageKabReserverJob);
        public const string RefreshWeeklyBetaLeaderboard = nameof(RefreshWeeklyBetaLeaderboard);
        public const string RefreshPveClimbLeaderboard = nameof(RefreshPveClimbLeaderboard);
        public const string ClearMatchReplay = nameof(ClearMatchReplay);
    }

    public static class SystemSubject
    {
        public const string PancakeExchange = nameof(PancakeExchange);
        public const string BetaLeaderboard = nameof(BetaLeaderboard);
        public const string WeeklyBetaLeaderboard = nameof(WeeklyBetaLeaderboard);
        public const string PveClimbLeaderboard = nameof(PveClimbLeaderboard);
        public const string User = nameof(User);
        public const string Hero = nameof(Hero);
        public const string AutoResult = nameof(AutoResult);
        public const string Shop = nameof(Shop);
        public const string Mail = nameof(Mail);
        public const string Mp = nameof(Mp);
        public const string Breed = nameof(Breed);
        public const string Presence = nameof(Presence);
        public const string StageKabReserver = nameof(StageKabReserver);
    }
}
