using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        private readonly ICacheService _cache;

        public LinkService(ILinkRepository linkRepository, ICacheService cache)
        {
            _linkRepository = linkRepository;
            _cache = cache;
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
        public async Task<LinkResponse> Get(string shortCode)
        {
            var entity = await _cache.Get<LinkEntity>(shortCode);

            // ReSharper disable once InvertIf
            if (entity is null)
            {
                entity = _linkRepository.Get(shortCode);
                await _cache.Set(shortCode, entity);
            }

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
        public async Task<string> Create([DisallowNull] LinkRequest linkRequest)
        {
            var isExistInReserveCollection = Common.GetReserveKeywordList().Any(i => i.Contains(linkRequest.ShortCode));
            if (isExistInReserveCollection) throw new KeyException(linkRequest.ShortCode, KeyExceptionType.Duplicate);

            // Check if it exist in DB
            var oldEntity = _linkRepository.Get(linkRequest.ShortCode);

            if (oldEntity is not null) throw new KeyException(linkRequest.ShortCode, KeyExceptionType.Duplicate);

            var entity = new LinkEntity(linkRequest.ShortCode, linkRequest.Url)
            {
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = linkRequest.ExpiredAt
            };

            // Insert into DB
            var insertLink = await _linkRepository.InsertLink(entity);
            
            // Insert into Cache too
            await _cache.Set(linkRequest.ShortCode, insertLink);

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
