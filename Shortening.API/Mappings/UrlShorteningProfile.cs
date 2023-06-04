using AutoMapper;
using Shortening.API.Dtos.RequestDtos;
using Shortening.API.Dtos.ResponseDtos;
using Shortening.API.Entities;

namespace Shortening.API.Mappings
{
    public class UrlShorteningProfile : Profile
    {
        public UrlShorteningProfile()
        {
            CreateMap<UrlShorteningEntity, GetUrlShorteningRequestDto>().ReverseMap();
            CreateMap<UrlShorteningEntity, CreateUrlShorteningRequestDto>().ReverseMap();
            CreateMap<UrlShorteningEntity, UrlShorteningResponseDto>().ReverseMap();

            CreateMap<GetUrlShorteningRequestDto, CreateUrlShorteningRequestDto>().ReverseMap();
        }
    }
}
