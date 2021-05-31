using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

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

        public static void ThrowWhenParameterIsNull([NotNull] object entity, string parameterName = "")
        {
            // TODO: Change this Exception, instead throw Custom API Exception and handle it in Global Exception handler.

            switch (entity)
            {
                case string str when string.IsNullOrEmpty(str):
                    throw new ArgumentNullException(parameterName);
                case null:
                    throw new ArgumentNullException(parameterName);
            }
        }

        [return: NotNull]
        public static List<string> GetReserveKeywordList() => new()
        {
            "links",
        };
    }
}
