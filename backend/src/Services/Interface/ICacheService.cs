using System.Threading.Tasks;

namespace ShortLink.Services.Interface
{
    public interface ICacheService
    {
        Task<T> Get<T>(string key) where T : class;

        Task Set<T>(string key, T entity) where T : class;
    }
}
