using System;

namespace GSBMaas.Models
{
    public class YouTubeVideo
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime PublishedAt { get; set; }
    }
} 