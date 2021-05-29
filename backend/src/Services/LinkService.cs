using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShortLink.Enums;
using ShortLink.Exceptions;
using ShortLink.Models;
using ShortLink.Models.Request;
using ShortLink.Models.Response;
using ShortLink.Repositories.Interfaces;
using ShortLink.Services.Interface;

namespace ShortLink.Services
{
    public class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;

        public LinkService(ILinkRepository linkRepository)
        {
            _linkRepository = linkRepository;
        }

        /// <inheritdoc />
        public IEnumerable<LinkResponse> GetAll()
        {
            return _linkRepository.GetAll().Select(l => new LinkResponse
            {
                ShortCode = l.RowKey,
                CreatedAt = l.CreatedAt,
                ExpiredAt = l.ExpiredAt,
                UpdatedAt = l.UpdatedAt,
                Url = l.Url
            });
        }

        /// <inheritdoc />
        public LinkResponse Get(string shortCode)
        {
            var entity = _linkRepository.Get(shortCode);
            return new LinkResponse
            {
                ShortCode = entity.RowKey,
                CreatedAt = entity.CreatedAt,
                ExpiredAt = entity.ExpiredAt,
                UpdatedAt = entity.UpdatedAt,
                Url = entity.Url
            };
        }

        /// <inheritdoc />
        public async Task<string> Create(LinkRequest linkRequest)
        {
            // TODO: Move the Validation in Repo here
            var entity = new LinkEntity(linkRequest.ShortCode, linkRequest.Url)
            {
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = linkRequest.ExpiredAt
            };

            await _linkRepository.InsertLink(entity);

            return linkRequest.ShortCode;
        }

        /// <inheritdoc />
        public async Task<string> Update(LinkRequest linkRequest)
        {
            var oldEntity = _linkRepository.Get(linkRequest.ShortCode) ??
                            throw new KeyException(linkRequest.ShortCode, KeyExceptionType.NotFound);

            oldEntity.Url = linkRequest.Url;
            oldEntity.ExpiredAt = linkRequest.ExpiredAt;
            oldEntity.UpdatedAt = DateTime.UtcNow;

            await _linkRepository.UpdateLink(oldEntity);
            return linkRequest.ShortCode;
        }

        /// <inheritdoc />
        public async Task Delete(string shortCode)
        {
            // TODO: Check the isDeleted value and throw an exception.
            var isDeleted = await _linkRepository.DeleteLink(shortCode);
        }
    }
}
