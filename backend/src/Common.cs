using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using ShortLink.Exceptions;

namespace ShortLink
{
    public static class Common
    {
        public static CloudTableClient GetTableClient(IConfiguration configuration)
        {
            // Table API - https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21#table-api
            var connectionString = configuration.GetConnectionString("CosmosDB");
            var account = CloudStorageAccount.Parse(connectionString);
            return account.CreateCloudTableClient();
        }

        public static void AssertNotNull(object entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));
        }

        /// <summary>
        /// Check if entity type is NULL and also check for NULL or EMPTY if of type string.
        /// </summary>
        /// <param name="entity">Parameter</param>
        /// <param name="parameterName">Parameter name</param>
        public static void ThrowWhenParameterIsNullOrIsEmpty([NotNull] object entity, string parameterName = "")
        {
            switch (entity)
            {
                case string str when string.IsNullOrEmpty(str):
                    throw new InvalidRequestException(parameterName);
                case null:
                    throw new InvalidRequestException(parameterName);
            }
        }

        [return: NotNull]
        public static List<string> GetReserveKeywordList() => new()
        {
            "get",
            "post",
            "put",
            "patch",
            "links"
        };
    }
}
