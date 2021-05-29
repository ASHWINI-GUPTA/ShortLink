using System;
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
    }
}
