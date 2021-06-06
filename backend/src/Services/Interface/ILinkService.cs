using System.Collections.Generic;
using System.Threading.Tasks;
using ShortLink.Models.Request;
using ShortLink.Models.Response;

namespace ShortLink.Services.Interface
{
    /// <summary>
    /// Handle operation related to Link
    /// </summary>
    public interface ILinkService
    {
        /// <summary>
        /// Get all Links
        /// </summary>
        /// <returns>Return collection of <see cref="LinkResponse"/>.</returns>
        IEnumerable<LinkResponse> GetAll();

        /// <summary>
        /// Get Link
        /// </summary>
        /// <returns>Return <see cref="LinkResponse"/>.</returns>
        Task<LinkResponse> Get(string shortCode);

        /// <summary>
        /// Create Link
        /// </summary>
        /// <returns>Return Short Code of newly created Link.</returns>
        Task<string> Create(LinkRequest linkRequest);

        /// <summary>
        /// Update Link
        /// </summary>
        /// <returns>Return Short Code of updated Link.</returns>
        Task<string> Update(LinkRequest linkRequest);

        /// <summary>
        /// Delete Link
        /// </summary>
        Task Delete(string shortCode);
    }
}
