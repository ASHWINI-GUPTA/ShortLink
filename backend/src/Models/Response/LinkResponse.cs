using System;

namespace ShortLink.Models.Response
{
    public class LinkResponse
    {
        public string ShortCode { get; set; }

        public string Url { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ExpiredAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
