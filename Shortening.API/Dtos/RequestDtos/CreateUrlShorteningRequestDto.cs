namespace Shortening.API.Dtos.RequestDtos
{
    public class CreateUrlShorteningRequestDto
    {
        public string? OriginalUrl { get; set; }
        public string? OptionalCustomShortenedUrl { get; set; }
    }
}
