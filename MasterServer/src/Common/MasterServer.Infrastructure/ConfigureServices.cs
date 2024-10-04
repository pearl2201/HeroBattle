using Amazon.CloudFront;
using Amazon.S3;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Iap.Verify;
using Iap.Verify.Models;
using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;
using MasterServer.Infrastructure.Files;
using MasterServer.Infrastructure.HealthChecks;
using MasterServer.Infrastructure.Identity;
using MasterServer.Infrastructure.Persistence;
using MasterServer.Infrastructure.Persistence.Interceptors;
using MasterServer.Infrastructure.Services;
using MasterServer.Infrastructure.Services.Aws;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Npgsql;
using Prometheus;
using Quartz;
using Quartz.AspNetCore;
using Quartz.Impl.AdoJobStore;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;
public static class ConfigureServices
{

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddAutoMapper((serviceProvider, automapper) =>
        {
            automapper.AddCollectionMappers();
            automapper.UseEntityFrameworkCoreModel<ApplicationDbContext>(serviceProvider);
            //automapper.UseEntityFrameworkCoreModel(serviceProvider);
        }, Assembly.GetExecutingAssembly());
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"));
        dataSourceBuilder.UseNodaTime();
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(dataSource,
                builder =>
                {
                    builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    builder.UseNodaTime();


                });
            if (env.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });


        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();



        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();


        services.AddFeatureManagement();
        //services.AddAuthorization(options =>
        //    options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        services.AddAwsInfrastructure(configuration, false);
        services.Configure<ServerSetting>(configuration.GetSection("ServerSetting"));


        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
        services.AddHttpClient();
        services.AddRedis(configuration);
        services.AddScoped<ICacheHelperService, RedisCacheService>();
        services.AddScoped<IRandomService, RandomService>();
        services.AddFeatureManagement(configuration.GetSection("FeatureFlags"));
        services.AddDataProtection().PersistKeysToDbContext<ApplicationDbContext>();
        services.AddHostedService<ApplicationLifetimeService>();
        services.Configure<HostOptions>(opts => opts.ShutdownTimeout = TimeSpan.FromSeconds(45));


        services.AddSingleton(serviceProvider =>
        {
            var _settings = serviceProvider.GetService<IOptions<ServerSetting>>().Value;
            ServiceAccountCredential credential;
            using (var stream = new FileStream(_settings.GCloudServiceAccountKeyPath, FileMode.Open, FileAccess.Read))
            {
                credential = ServiceAccountCredential.FromServiceAccountData(stream);
                credential.Scopes = credential.Scopes.Append(AndroidPublisherService.Scope.Androidpublisher);
            }
            return credential;
        });

        services.AddSingleton(serviceProvider =>
        {
            return new AndroidPublisherService(
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = serviceProvider.GetService<ServiceAccountCredential>(),
                    ApplicationName = "Herobase",
                }
            );
        });
        services.AddOptions<AppleSecretOptions>()
          .Configure<IConfiguration>((settings, config) => config.GetSection(AppleSecretOptions.AppleSecretStoreKey).Bind(settings));
        services.AddOptions<AppleStoreOptions>()
            .Configure<IConfiguration>((settings, config) => config.GetSection(AppleStoreOptions.AppleStoreKey).Bind(settings));
        services.AddOptions<GoogleOptions>()
            .Configure<IConfiguration>((settings, config) => config.GetSection(GoogleOptions.GoogleKey).Bind(settings));
        services.AddKeyedScoped<IIapVerify, GoogleVerify>("Google");
        services.AddKeyedScoped<IIapVerify, AppleVerifyReceipt>("Apple");

        return services;
    }

    public static IServiceCollection AddWebInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddInfrastructureServices(configuration, env);
        services.AddHealthCheckK8s(configuration);



        return services;
    }
    public static IServiceCollection AddAwsInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isLocal = true)//, IWebHostEnvironment environment)
    {
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonS3>();
        services.AddAWSService<IAmazonCloudFront>();
        services.AddScoped<IStorageService, AwsStorageService>();
        services.AddScoped<IWebsocketNofiticationService, WebsocketNofiticationService>();
        return services;
    }

    public static void AddHealthCheckK8s(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<StartupHostedService>();
        //services.AddHostedService<StartupFeatureFlagHostedService>();
        services.AddSingleton<StartupHostedServiceHealthCheck>();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>(tags: new[] { "ready" }, customTestQuery: QueryTestDb)
            //.AddCheck<CustomDbContextCheck>("custom_db_context_check", failureStatus: HealthStatus.Unhealthy,
            //    tags: new[] { "ready" })
            .ForwardToPrometheus()
            .AddCheck<StartupHostedServiceHealthCheck>(
                "hosted_service_startup",
                failureStatus: HealthStatus.Degraded,
                tags: new[] { "ready" });

        services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Delay = TimeSpan.FromSeconds(2);
            options.Predicate = (check) => check.Tags.Contains("ready");
        });

        services.AddSingleton<IHealthCheckPublisher, ReadinessPublisher>();
    }

    public static async Task<bool> QueryTestDb(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        await dbContext.Players.AnyAsync(cancellationToken);
        return true;
    }

    public static void MapEndPointHealthCheckK8s(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
        {
            Predicate = (check) => check.Tags.Contains("ready"),
        });

        endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
        {
            Predicate = (_) => false
        });
    }


    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfiguration = configuration.GetSection("Redis").Get<RedisConfiguration>();
        _ = services.AddSingleton<RedisConfiguration>(redisConfiguration);

        services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);
        var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
        };
        jsonSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        NewtonsoftSerializer newtonsoftSerializer = new NewtonsoftSerializer(jsonSettings);
        services.AddSingleton<ISerializer>(newtonsoftSerializer);

    }


    // note: we need configure quarzt to help admin could work with it
    public static IServiceCollection AddWorkerJob(this IServiceCollection services, IConfiguration configuration, bool isWorker)
    {
        services.AddQuartz(q =>
        {
            // base quartz scheduler, job and trigger configuration

            // handy when part of cluster or you want to otherwise identify multiple schedulers
            q.SchedulerId = "Scheduler-Core";

            // we take this from appsettings.json, just show it's possible
            // q.SchedulerName = "Quartz ASP.NET Core Sample Scheduler";

            // as of 3.3.2 this also injects scoped services (like EF DbContext) without problems
            q.UseMicrosoftDependencyInjectionJobFactory();

            // or for scoped service support like EF Core DbContext
            // q.UseMicrosoftDependencyInjectionScopedJobFactory();

            // these are the defaults
            q.UseSimpleTypeLoader();
            q.UsePersistentStore(c =>
            {
                //c.UseProperties = true;
                // Use for PostgresSQL database
                c.UsePostgres(postgresOptions =>
                {
                    postgresOptions.UseDriverDelegate<PostgreSQLDelegate>();
                    postgresOptions.ConnectionString = configuration.GetConnectionString("DefaultConnection");
                    postgresOptions.TablePrefix = "quartz.qrtz_";
                });

                c.UseNewtonsoftJsonSerializer();
            });

            q.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = 10;
            });

            //http://www.cronmaker.com/?0
            //q.ScheduleJob<RefetchFirebaseLeaderboardJob>(trigger => trigger
            //   .WithIdentity("RefetchFirebaseLeaderboardJob_at_1_minutes", "production_leaderboard")
            //   .StartNow()
            //   .WithSimpleSchedule((y) =>
            //   {
            //       y.WithIntervalInMinutes(1).RepeatForever();
            //   })
            //   .WithDescription("Refetch Firebase Leaderboard Job every 1 minutes"));

            //q.ScheduleJob<DetectAndPromoteLeagueJob>(trigger => trigger
            //   .WithIdentity("Act Promoto Language Job", "Act promote language job")
            //    .WithCronSchedule("0 0/1 * 1/1 * ? *", x => x
            //    .InTimeZone(TimeZoneInfo.Utc))
            //    .WithDescription("DetectAndPromoteLeagueJob every 1 minutes"));

            //q.ScheduleJob<RefreshLeagueMiniLeaderboardJob>(trigger => trigger
            //   .WithIdentity("RefreshLeagueMiniLeaderboardJob_at_1_minutes", "production_leaderboard")
            //   .StartNow()
            //   .WithSimpleSchedule((y) =>
            //   {
            //       y.WithIntervalInMinutes(1).RepeatForever();
            //   })
            //   .WithDescription("RefreshLeagueMiniLeaderboardJob every 1 minutes"));

            //q.ScheduleJob<DetectBattleScoreCheatingJob>(trigger => trigger
            //   .WithIdentity("DetectBattleScoreCheatingJob_at_30_minutes", "production_battle")
            //   .StartNow()
            //   .WithSimpleSchedule((y) =>
            //   {
            //       y.WithIntervalInMinutes(30).RepeatForever();
            //   })
            //   .WithDescription("Detech cheat standing every 30"));
            //q.ScheduleJob<IncreaseLeagueEventBotCrownJob>(trigger => trigger
            // .WithIdentity("IncreaseLeagueEventBotCrownJob_at_every_1_hours", "Act promote language job")
            //      .WithCronSchedule("0 0 0/1 1/1 * ? *", x => x
            //    .InTimeZone(TimeZoneInfo.Utc))
            //    .WithDescription("IncreaseLeagueEventBotCrownJob every 1 hours"));






            //q.ScheduleJob<ClearArchivedTableThatExistedEalierThan2MonthsAgoJob>(trigger => trigger
            //    .WithIdentity("ClearArchivedTableThatExistedEalierThan2MonthsAgoJob_everyday", "leagues")
            //    .WithCronSchedule("0 0 12 1/1 * ? *", x => x
            //    .InTimeZone(TimeZoneInfo.Utc))
            //    .WithDescription("ClearArchivedTableThatExistedEalierThan2MonthsAgoJob_everyday"));

            //q.ScheduleJob<ClearStaleWsSession>(trigger => trigger
            //  .WithIdentity("ClearStaleWsSession_everyday", "daily")
            //  .WithCronSchedule("0 0 0/1 1/1 * ? *", x => x
            //  .InTimeZone(TimeZoneInfo.Utc))
            //  .WithDescription("ClearStaleWsSession_everyday"));


            //q.ScheduleJob<PruneDanglingGameMatch>(trigger =>
            //    trigger
            //    .WithIdentity("prune_dangling_gamematch_every_minutes", "game")
            //    .WithCronSchedule("0 0/1 * 1/1 * ? *", x => x.InTimeZone(TimeZoneInfo.Utc))
            //    .WithDescription("PruneDanglingGameMatch")

            //);

            //q.ScheduleJob<ClearGameSessionsJob>(trigger =>
            //    trigger
            //    .WithIdentity("ClearGameSessionsJob_every_days", "game")
            //    .WithCronSchedule("0 0 12 1/1 * ? *", x => x.InTimeZone(TimeZoneInfo.Utc))
            //    .WithDescription("ClearGameSessionsJob")

            //);

            //q.ScheduleJob<PruneOldGameMatchJob>(trigger =>
            //    trigger
            //    .WithIdentity("PruneOldGameMatchJob_every_days", "game")
            //    .WithCronSchedule("0 0 12 1/1 * ? *", x => x.InTimeZone(TimeZoneInfo.Utc))
            //    .WithDescription("PruneOldGameMatchJob")

            //);

            //q.ScheduleJob<ClearPlayerActionLogsJob>(trigger =>
            //    trigger
            //    .WithIdentity("ClearPlayerActionLogsJob_every_days", "game")
            //    .WithCronSchedule("0 0 12 1/1 * ? *", x => x.InTimeZone(TimeZoneInfo.Utc))
            //    .WithDescription("ClearPlayerActionLogsJob")

            //);

            //q.AddJob<ImportGameConfigJob>(opts =>
            //{
            //    var jobKey = new JobKey(AdminJob.ImportExportData.ToString(), "admin");
            //    opts.WithIdentity(jobKey);
            //    opts.StoreDurably(true);
            //});


        });

        if (isWorker)
        {
            // ASP.NET Core hosting
            services.AddQuartzServer(options =>
            {
                options.AwaitApplicationStarted = true;
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
        }


        return services;
    }


}
