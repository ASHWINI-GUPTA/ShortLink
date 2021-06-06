using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    /// <summary>
    /// Perform DB operations on <see cref="LinkEntity"/>
    /// </summary>
    public class LinkRepository : ILinkRepository
    {
        private readonly CloudTable _table;

        public LinkRepository(IConfiguration configuration)
        {
            var tableClient = Common.GetTableClient(configuration);
            _table = tableClient.GetTableReference("links");
        }

        /// <inheritdoc />
        public IEnumerable<LinkEntity> GetAll()
        {
            var tableQuery = new TableQuery<LinkEntity>();
            return _table.ExecuteQuery(tableQuery);
        }

        /// <inheritdoc />
        public LinkEntity Get([DisallowNull] string shortCode)
        {
            var tableQuery = new TableQuery<LinkEntity>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "slink"),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, shortCode)));

            return _table.ExecuteQuery(tableQuery)?.FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<LinkEntity> Insert([DisallowNull] LinkEntity entity)
        {
            var tableResult = await _table.ExecuteAsync(TableOperation.Insert(entity));
            return tableResult.Result as LinkEntity;
        }

        /// <inheritdoc />
        public async Task<LinkEntity> Update([DisallowNull] LinkEntity entity)
        {
            var op = TableOperation.Merge(entity);
            var result = await _table.ExecuteAsync(op);
            return result.Result as LinkEntity;
        }

        /// <inheritdoc />
        public async Task Remove([DisallowNull] LinkEntity entity)
        {
            var result = await _table.ExecuteAsync(TableOperation.Delete(entity));
            
            if (result.HttpStatusCode is not (>= 200 and < 300))
            {
                throw new OperationException(Operation.DeleteLink, result.Result.ToString());
            }
        }
    }
}