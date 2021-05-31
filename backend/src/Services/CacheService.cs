using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using ShortLink.Services.Interface;

namespace ShortLink.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> Get<T>(string key) where T : class
        {
            var entity = await _cache.GetStringAsync(key);
            return entity is null ? null : JsonSerializer.Deserialize<T>(entity);
        }

        public async Task Set<T>(string key, T entity) where T : class
        {
            var cacheOption = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(7) };
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(entity), cacheOption);
        }
    }
}