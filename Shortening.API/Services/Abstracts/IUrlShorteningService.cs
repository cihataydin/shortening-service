using AutoMapper;
using Shortening.API.Contexts;
using Shortening.API.Dtos.RequestDtos;
using Shortening.API.Dtos.ResponseDtos;
using Shortening.API.Entities;
using Shortening.API.Repositories.Abstracts;
using Shortening.API.UnitOfWorks.Abstracts;

namespace Shortening.API.Services.Abstracts
{
    public interface IUrlShorteningService
    {
        Task<UrlShorteningResponseDto> CreateUrlShorteningAsync(CreateUrlShorteningRequestDto requestDto);
        Task<UrlShorteningResponseDto> GetUrlShorteningForAsync(GetUrlShorteningRequestDto requestDto);
    }
}
