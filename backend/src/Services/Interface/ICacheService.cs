using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace ShortLink.Services.Interface
{
    /// <summary>
    /// Handle Cache specific operations
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Get the object from Cache for a given key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Key</param>
        /// <returns>Return object for specified key if not else NULL.</returns>
        [return: MaybeNull]
        Task<T?> Get<T>(string key) where T : class;

        /// <summary>
        /// Set the object in Cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Key</param>
        /// <param name="entity">Entity to store in Cache</param>
        Task Set<T>(string key, T entity) where T : class;

        /// <summary>
        /// Remove the object from Cache if found based on key.
        /// </summary>
        /// <param name="key">Key</param>
        Task Remove(string key);
    }
}
