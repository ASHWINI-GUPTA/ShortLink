using System;

namespace ShortLink.Models.Request
{
    public class LinkRequest
    {
        public string ShortCode { get; set; }

        public string Url { get; set; }

        public DateTime? ExpiredAt { get; set; }
    }
}
