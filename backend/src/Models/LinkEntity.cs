using System;
using Microsoft.Azure.Cosmos.Table;

namespace ShortLink.Models
{
    public class LinkEntity : TableEntity
    {
        public LinkEntity()
        {
        }

        public LinkEntity(string shortCode, string url)
        {
            RowKey = shortCode;
            PartitionKey = "slink";
            Url = url;
        }

        // TODO Add Auth
        public int UserId { get; set; }

        public string Url { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ExpiredAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}