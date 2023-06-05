using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shortening.API.Adapters;
using Shortening.API.Constants;
using Shortening.API.Contexts;
using Shortening.API.Dtos.RequestDtos;
using Shortening.API.Dtos.ResponseDtos;
using Shortening.API.Entities;
using Shortening.API.Repositories.Abstracts;
using Shortening.API.Services.Abstracts;
using Shortening.API.UnitOfWorks.Abstracts;
using System.Web;

namespace Shortening.API.Services.Concretes
{
    public class UrlShorteningService : IUrlShorteningService
    {
        private readonly IEFRepository<UrlShorteningEntity, ShorteningDbContext> _urlShorteningRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CahceAdapter _cahceAdapter;

        public UrlShorteningService(IEFRepository<UrlShorteningEntity, ShorteningDbContext> urlShorteningRepository,
            IUnitOfWork unitOfWork, IMapper mapper, CahceAdapter redisCahceAdapter)
        {
            _urlShorteningRepository = urlShorteningRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cahceAdapter = redisCahceAdapter;
        }

        public async Task<UrlShorteningResponseDto> GetUrlShorteningForAsync(GetUrlShorteningRequestDto requestDto)
        {
            requestDto.ShortenedUrl = CheckShorteningUrl(requestDto.ShortenedUrl, out string code);

            var cachedOriginalUrl = _cahceAdapter.GetCachedOriginalUrl(code);

            if (!string.IsNullOrEmpty(cachedOriginalUrl))
            {
                return new UrlShorteningResponseDto { OriginalUrl = cachedOriginalUrl, ShortenedUrl = requestDto.ShortenedUrl };
            }

             var query = _urlShorteningRepository.Get();

            if (!string.IsNullOrWhiteSpace(requestDto.OriginalUrl))
                query = query.Where(u => u.OriginalUrl == requestDto.OriginalUrl);
            if (!string.IsNullOrWhiteSpace(requestDto.ShortenedUrl))
                query = query.Where(u => u.ShortenedUrl == requestDto.ShortenedUrl);
            
            var urlShorteningEntity = await query.FirstOrDefaultAsync();

            var result = _mapper.Map<UrlShorteningResponseDto>(urlShorteningEntity);

            if(result is not null)
                CheckShorteningUrl(result.ShortenedUrl, out code);

            _cahceAdapter.CacheOriginalUrl(code, result?.OriginalUrl);

            return result;
        }

        public async Task<UrlShorteningResponseDto> CreateUrlShorteningAsync(CreateUrlShorteningRequestDto requestDto)
        {
            var getRequestDto = _mapper.Map<GetUrlShorteningRequestDto>(requestDto);

            var urlShorteningResponseDto = await GetUrlShorteningForAsync(getRequestDto);

            if (urlShorteningResponseDto is not null)
            {
                urlShorteningResponseDto.IsExists = true;

                return urlShorteningResponseDto;
            }

            var urlShorteningEntity = _mapper.Map<UrlShorteningEntity>(requestDto);

            urlShorteningEntity.ShortenedUrl = requestDto.OptionalCustomShortenedUrl is not null 
                ? GenerateShortUrl(requestDto.OptionalCustomShortenedUrl) : GenerateShortUrl();

            urlShorteningEntity = await _urlShorteningRepository.CreateAsync(urlShorteningEntity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UrlShorteningResponseDto>(urlShorteningEntity);
        }

        private string? GenerateShortUrl(string? customUrl = null)
        {
            var randomBytes = BitConverter.GetBytes(new Random().NextInt64() + 1);

            var hashedUrl = customUrl is not null 
                ? customUrl : $"{Convert.ToBase64String(randomBytes).Replace("/", "_").Replace("+", "-").Substring(0, 6)}/";

            return ShorteningConstants.SHORTENING_DOMAIN_URL.EndsWith("/") 
                ? $"{ShorteningConstants.SHORTENING_DOMAIN_URL}{hashedUrl}" : $"{ShorteningConstants.SHORTENING_DOMAIN_URL}/{hashedUrl}"; 
        }

        private string CheckShorteningUrl(string url, out string code)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                if (url.Length > 6)
                {
                    url = HttpUtility.UrlDecode(url).Replace("\0", "/");

                    code = url.Remove(0, ShorteningConstants.SHORTENING_DOMAIN_URL.Length).TrimEnd('/');
                }
                else
                {
                    code = url;

                    url = ShorteningConstants.SHORTENING_DOMAIN_URL.EndsWith("/") 
                        ? $"{ShorteningConstants.SHORTENING_DOMAIN_URL}{url}/" : $"{ShorteningConstants.SHORTENING_DOMAIN_URL}/{url}/";
                }
            }
            else
            {
                code = default;
            }

            return url;
        }
    }
}
