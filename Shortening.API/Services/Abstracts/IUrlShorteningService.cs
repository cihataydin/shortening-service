using Shortening.API.Dtos.RequestDtos;
using Shortening.API.Dtos.ResponseDtos;


namespace Shortening.API.Services.Abstracts
{
    public interface IUrlShorteningService
    {
        Task<UrlShorteningResponseDto> CreateUrlShorteningAsync(CreateUrlShorteningRequestDto requestDto);
        Task<UrlShorteningResponseDto> GetUrlShorteningForAsync(GetUrlShorteningRequestDto requestDto);
    }
}
