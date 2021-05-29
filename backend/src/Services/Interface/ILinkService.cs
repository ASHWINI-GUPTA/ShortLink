using System.Collections.Generic;
using System.Threading.Tasks;
using ShortLink.Models.Request;
using ShortLink.Models.Response;

namespace ShortLink.Services.Interface
{
    public interface ILinkService
    {
        IEnumerable<LinkResponse> GetAll();

        LinkResponse Get(string shortCode);

        Task<string> Create(LinkRequest linkRequest);

        Task<string> Update(LinkRequest linkRequest);

        Task Delete(string shortCode);
    }
}
