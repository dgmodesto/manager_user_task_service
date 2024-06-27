using StackExchange.Redis;
using System.Text.Json;

namespace ManagerUserTaskApi.Infrastructure.Cache;

public class CacheService<TModel> : ICacheService<TModel> where TModel : class
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public CacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }


    public async Task SetStringAsync(string key, string value, TimeSpan? expiry = null)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.StringSetAsync(key, value, expiry);
    }

    public async Task<TModel?> GetStringAsync<TModel>(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var cache = await db.StringGetAsync(key);

        if (!string.IsNullOrEmpty(cache))
            return JsonSerializer.Deserialize<TModel>(cache);

        return default(TModel?);
    }

}
