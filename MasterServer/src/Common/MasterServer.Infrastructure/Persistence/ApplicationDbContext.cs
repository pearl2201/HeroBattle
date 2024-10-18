using AppAny.Quartz.EntityFrameworkCore.Migrations;
using AppAny.Quartz.EntityFrameworkCore.Migrations.PostgreSQL;
using MasterServer.Application.Common.Interfaces;
using MasterServer.Domain.Entities;
using MasterServer.Domain.Entities.BattlePasses;
using MasterServer.Domain.Entities.Game;
using MasterServer.Domain.Entities.GameNotification;
using MasterServer.Domain.Entities.GameSave;
using MasterServer.Domain.Entities.Leagues;
using MasterServer.Domain.Entities.Mails;
using MasterServer.Domain.Entities.Socials;
using MasterServer.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace MasterServer.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext, IDataProtectionKeyContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    private bool DisposedEntities { get; set; }




    public virtual DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public ApplicationDbContext()
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.AddQuartz(builder => builder.UsePostgreSql());
        base.OnModelCreating(builder);

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);

    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }

    private static void DisposeEntities<T>(DbSet<T> dbSet)
where T : class, IDisposable
    {
        try
        {
            foreach (var entity in dbSet.Local)
            {

                entity.Dispose();

            }
        }
        catch (Exception)
        {
            //Console.WriteLine("Error on dispose object");
        }
    }

    public virtual void SetModified(object entity)
    {
        Entry(entity).State = EntityState.Modified;
    }

    public virtual IDbContextTransaction StartTransaction(System.Data.IsolationLevel isolationLevel)
    {
        return Database.BeginTransaction(isolationLevel);
    }



    public virtual void SetPropertyModified<T>(T entity, string selectProperty)
    {
        var property = Entry(entity).Property(selectProperty);
        if (property != null)
            property.IsModified = true;
    }

    public virtual void ClearChangeTracker()
    {
        ChangeTracker.Clear();
    }

    public IDbConnection DbConnection => Database.GetDbConnection();

    public virtual DbSet<Player> Players { get; set; }
    public virtual DbSet<GameSession> GameSessions { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
   
    public virtual DbSet<GameMatch> GameMatches { get; set; }
  
    public virtual DbSet<GameNotification> GameNotifications { get; set; }
    public virtual DbSet<GameMail> GameMails { get; set; }
    public virtual DbSet<MailAttachment> MailAttachments { get; set; }
    public virtual DbSet<PlayerMail> PlayerMails { get; set; }
 
    public virtual DbSet<Conversation> Conversations { get; set; }
    public virtual DbSet<ConversationMessage> ConversationMessages { get; set; }
    public virtual DbSet<ConversationParticipant> ConversationParticipants { get; set; }
  
    public virtual DbSet<LeagueSeason> LeagueSeasons { get; set; }
    public virtual DbSet<LeagueSeasonParticipant> LeagueSeasonParticipants { get; set; }
    public virtual DbSet<LeagueSeasonRank> LeagueSeasonRanks { get; set; }
    public virtual DbSet<LeagueSeasonRankReward> LeagueSeasonRankRewards { get; set; }

    public virtual DbSet<GameMailTemplate> GameMailTemplates { get; set; }
  
    public virtual DbSet<LeagueSeasonRankLeaderboard> LeagueSeasonRankLeaderboards { get; set; }
   
    public virtual DbSet<PlayerBattleProfile> PlayerBattleProfiles { get; set; }
    
    public DbSet<GameKvSettingConfig> GameKvSettingConfigs { get; set; }
  
    public DbSet<LeagueSeasonParticipantRank> LeagueSeasonParticipantRanks { get; set; }
    public DbSet<PlayerShopProfile> PlayerShopProfiles { get; set; }
   
    public DbSet<BattlePassDefinition> BattlePassSeasons { get; set; }
    public DbSet<BattlePassSeasonParticipant> BattlePassParticipants { get; set; }
    public DbSet<BattlePassSeasonParticipantMilestone> BattlePassParticipantMilestones { get; set; }
    public DbSet<BattlePassMilestone> BattlePassMilestones { get; set; }
    public DbSet<PlayerEdge> PlayerEdges { get; set; }
}
