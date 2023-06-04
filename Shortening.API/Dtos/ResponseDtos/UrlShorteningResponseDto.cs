namespace Shortening.API.Dtos.ResponseDtos
{
    public class UrlShorteningResponseDto
    {
        public string? ShortenedUrl { get; set; }
        public string? OriginalUrl { get; set; }
        public bool IsExists { get; set; }
    }
}
