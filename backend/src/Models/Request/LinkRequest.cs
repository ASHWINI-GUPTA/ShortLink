using System;
using System.ComponentModel.DataAnnotations;

namespace ShortLink.Models.Request
{
    public class LinkRequest
    {
        public LinkRequest(string shortCode, string url)
        {
            ShortCode = shortCode;
            Url = url;
        }

        [Required]
        public string ShortCode { get; set; }

        [Required]
        public string Url { get; set; }

        public DateTime? ExpiredAt { get; set; }
    }
}
