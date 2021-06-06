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
    /// <inheritdoc cref="ILinkService"/>
    public class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;
        private readonly ICacheService _cache;

        /// <summary>
        /// Initialize new instance of <see cref="LinkService"/>
        /// </summary>
        /// <param name="linkRepository">Link Repository</param>
        /// <param name="cache">Cache Service</param>
        public LinkService(ILinkRepository linkRepository, ICacheService cache)
        {
            _linkRepository = linkRepository;
            _cache = cache;
        }

        /// <inheritdoc />
        public IEnumerable<LinkResponse> GetAll()
        {
            IEnumerable<LinkEntity> entities;
            try
            {
                entities = _linkRepository.GetAll();
            }
            catch (Exception exception)
            {
                throw new OperationException(Operation.GetLink, exception);
            }

            return entities.Select(l => new LinkResponse
            {
                ShortCode = l.RowKey,
                CreatedAt = l.CreatedAt,
                ExpiredAt = l.ExpiredAt,
                UpdatedAt = l.UpdatedAt,
                Url = l.Url
            });
        }

        /// <inheritdoc />
        public async Task<LinkResponse> Get([DisallowNull] string shortCode)
        {
            LinkEntity? entity;
            try
            {
                entity = await _cache.Get<LinkEntity>(shortCode);

                // ReSharper disable once InvertIf
                if (entity is null)
                {
                    entity = _linkRepository.Get(shortCode);
                    await _cache.Set(shortCode, entity);
                }
            }
            catch (Exception exception)
            {
                throw new OperationException(Operation.GetLink, exception);
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

            try
            {
                // Insert into DB
                var insertLink = await _linkRepository.Insert(entity);
                // Insert into Cache too
                await _cache.Set(linkRequest.ShortCode, insertLink);

                return linkRequest.ShortCode;
            }
            catch (Exception exception)
            {
                throw new OperationException(Operation.CreateLink, exception);
            }
        }

        /// <inheritdoc />
        public async Task<string> Update([DisallowNull] LinkRequest linkRequest)
        {
            var oldEntity = _linkRepository.Get(linkRequest.ShortCode) ??
                            throw new KeyException(linkRequest.ShortCode, KeyExceptionType.NotFound);

            oldEntity.Url = linkRequest.Url;
            oldEntity.ExpiredAt = linkRequest.ExpiredAt;
            oldEntity.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _linkRepository.Update(oldEntity);
                return linkRequest.ShortCode;
            }
            catch (Exception exception)
            {
                throw new OperationException(Operation.UpdateLink, exception);
            }
        }

        /// <inheritdoc />
        public async Task Delete([DisallowNull] string shortCode)
        {
            var oldLink = _linkRepository.Get(shortCode);

            if (oldLink is not null)
                throw new KeyException(shortCode, KeyExceptionType.NotFound);

            try
            {
                await _linkRepository.Remove(oldLink);
                await _cache.Remove(shortCode);
            }
            catch (Exception exception)
            {
                throw new OperationException(Operation.DeleteLink, exception);
            }
        }
    }
}
