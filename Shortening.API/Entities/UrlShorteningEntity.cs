namespace Shortening.API.Entities
{
    public class UrlShorteningEntity : BaseEntity
    {
        public string? OriginalUrl { get; set; }
        public string? ShortenedUrl { get; set; }
    }
}
