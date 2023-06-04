namespace Shortening.API.Dtos.RequestDtos
{
    public class GetUrlShorteningRequestDto
    {
        public string? OriginalUrl { get; set; }
        public string? ShortenedUrl { get; set; }
    }
}
