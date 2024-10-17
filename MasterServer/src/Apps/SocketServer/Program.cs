using Microsoft.Extensions.Configuration;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using SServer;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<SocketServer>();

var redisConfiguration = builder.Configuration.GetSection("Redis").Get<RedisConfiguration>();
builder.Services.AddSingleton(redisConfiguration);

builder.Services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);
var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings()
{
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
};
jsonSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
NewtonsoftSerializer newtonsoftSerializer = new NewtonsoftSerializer(jsonSettings);
builder.Services.AddSingleton<ISerializer>(newtonsoftSerializer);


var host = builder.Build();
host.Run();
