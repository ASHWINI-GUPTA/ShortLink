using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using ShortLink.Services.Interface;

namespace ShortLink.Services
{
    /// <inheritdoc cref="ICacheService"/>
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        /// <summary>
        /// Initialize new instance of <see cref="CacheService"/>
        /// </summary>
        /// <param name="cache">Cache Provider</param>
        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <inheritdoc />
        [return: MaybeNull]
        public async Task<T> Get<T>(string key) where T : class
        {
            var entity = await _cache.GetStringAsync(key);
            return entity is null ? null : JsonSerializer.Deserialize<T>(entity);
        }

        /// <inheritdoc />
        public async Task Set<T>(string key, T entity) where T : class
        {
            var cacheOption = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(7) };
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(entity), cacheOption);
        }

        /// <inheritdoc />
        public async Task Remove(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}