using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using ShortLink.Enums;
using ShortLink.Exceptions;
using ShortLink.Models;
using ShortLink.Repositories.Interfaces;

namespace ShortLink.Repositories
{
    public class LinkRepository : ILinkRepository
    {
        private readonly CloudTable _table;

        public LinkRepository(IConfiguration configuration)
        {
            var tableClient = Common.GetTableClient(configuration);
            _table = tableClient.GetTableReference("links");
        }

        public IEnumerable<LinkEntity> GetAll()
        {
            var tableQuery = new TableQuery<LinkEntity>();
            return _table.ExecuteQuery(tableQuery);
        }

        public LinkEntity Get(string shortCode)
        {
            var tableQuery = new TableQuery<LinkEntity>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "slink"),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, shortCode)));

            return _table.ExecuteQuery(tableQuery)?.FirstOrDefault();
        }

        public async Task<LinkEntity> InsertLink(LinkEntity entity)
        {
            var oldLink = Get(entity.RowKey);
            
            if (oldLink is not null) throw new KeyException(entity.RowKey, KeyExceptionType.Duplicate);

            var tableResult = await _table.ExecuteAsync(TableOperation.Insert(entity));
            return tableResult.Result as LinkEntity;
        }

        public async Task<LinkEntity> UpdateLink(LinkEntity entity)
        {
            var op = TableOperation.Merge(entity);
            var result = await _table.ExecuteAsync(op);
            return result.Result as LinkEntity;
        }

        public async Task<bool> DeleteLink(string key)
        {
            var oldLink = Get(key) ?? throw new KeyException(key, KeyExceptionType.NotFound);
            var result = await _table.ExecuteAsync(TableOperation.Delete(oldLink));
            return result.HttpStatusCode is >= 200 and < 300;
        }
    }
}