using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManager.Application.Services;

namespace TaskManager.Infrastructure.Cache
{
    public class RedisService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var jsonData = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, jsonData, expiration);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var jsonData = await _db.StringGetAsync(key);
            if (jsonData.IsNullOrEmpty)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
         
            var server = _redis.GetServer(_redis.GetEndPoints().First());

         
            var keys = server.Keys(pattern: $"{prefix}*");

            await _db.KeyDeleteAsync(keys.ToArray());
        }
    }
}
