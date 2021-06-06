using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ShortLink.Models;

namespace ShortLink.Repositories.Interfaces
{
    /// <summary>
    /// Define method will be available on <see cref="ILinkRepository"/> implementation.
    /// </summary>
    public interface ILinkRepository
    {
        /// <summary>
        /// Get collection of <see cref="LinkEntity"/> from DB.
        /// </summary>
        /// <returns>Collection of <see cref="LinkEntity"/></returns>
        IEnumerable<LinkEntity> GetAll();

        /// <summary>
        /// Get <see cref="LinkEntity"/> from DB.
        /// </summary>
        /// <param name="shortCode">Short Code of URL</param>
        /// <returns><see cref="LinkEntity"/> for a provided shortCode</returns>
        LinkEntity Get([DisallowNull] string shortCode);

        /// <summary>
        /// Create new <see cref="LinkEntity"/> in DB/.
        /// </summary>
        /// <param name="entity">Instance of <see cref="LinkEntity"/> to operate on.</param>
        /// <returns>Newly created <see cref="LinkEntity"/></returns>
        Task<LinkEntity> Insert([DisallowNull] LinkEntity entity);
        
        /// <summary>
        /// Update existing <see cref="LinkEntity"/> in DB.
        /// </summary>
        /// <param name="entity">Instance of <see cref="LinkEntity"/> to operate on.</param>
        /// <returns>Updated <see cref="LinkEntity"/></returns>
        Task<LinkEntity> Update([DisallowNull] LinkEntity entity);

        /// <summary>
        /// Remove existing <see cref="LinkEntity"/> in DB.
        /// </summary>
        /// <param name="entity">Instance of <see cref="LinkEntity"/> to operate on.</param>
        /// <returns><c>Boolean</c> value indicate whether <see cref="LinkEntity"/> is successfully deleted from DB or not.</returns>
        Task Remove([DisallowNull] LinkEntity entity);
    }
}
