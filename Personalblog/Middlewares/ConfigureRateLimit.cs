using StackExchange.Redis;
using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;

namespace Personalblog.Middlewares;

public static class ConfigureRateLimit
{
    public static void AddRateLimit(this IServiceCollection services,IConfiguration conf)
    {
        services.Configure<IpRateLimitOptions>(conf.GetSection("IpRateLimiting"));
        // 注册 Redis 缓存服务
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = conf.GetConnectionString("Redis");
        });
        // 注册 Redis 连接服务
        var redisOptions = ConfigurationOptions.Parse(conf.GetConnectionString("Redis"));
        redisOptions.Password = "zyplj1314999";
        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            return ConnectionMultiplexer.Connect(redisOptions);
        });
        services.AddRedisRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    }
    public static IApplicationBuilder UseRateLimit(this IApplicationBuilder app)
    {
        app.UseIpRateLimiting();
        return app;
    }
}