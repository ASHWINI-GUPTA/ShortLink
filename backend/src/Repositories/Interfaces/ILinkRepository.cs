using System.Collections.Generic;
using System.Threading.Tasks;
using ShortLink.Models;

namespace ShortLink.Repositories.Interfaces
{
    public interface ILinkRepository
    {
        IEnumerable<LinkEntity> GetAll();
        Task<LinkEntity> InsertLink(LinkEntity entity);
        Task<LinkEntity> UpdateLink(LinkEntity entity);
        Task<bool> DeleteLink(string key);
        LinkEntity Get(string shortCode);
    }
}
