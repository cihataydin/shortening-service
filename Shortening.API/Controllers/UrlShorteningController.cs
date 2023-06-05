using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Shortening.API.Dtos.RequestDtos;
using Shortening.API.Services.Abstracts;

namespace Shortening.API.Controllers
{
    [ApiController]
    [Route("")]
    public class UrlShorteningController : ControllerBase
    {
        private readonly IUrlShorteningService _urlShorteningService;
        private readonly IValidator<CreateUrlShorteningRequestDto> _validator;
        public UrlShorteningController(IUrlShorteningService urlShorteningService, IValidator<CreateUrlShorteningRequestDto> validator)
        {
            _urlShorteningService = urlShorteningService;
            _validator = validator;
        }

        [HttpGet("{shortenUrlOrCode}")]
        public async Task<IActionResult> RedirectTo(string shortenUrlOrCode)
        {
            var urlShortening = await _urlShorteningService.GetUrlShorteningForAsync(new GetUrlShorteningRequestDto { ShortenedUrl = shortenUrlOrCode });

            if (urlShortening != null)
            {
                return Redirect(urlShortening.OriginalUrl);
            }

            return NotFound("Given shortened url or code is not found.");
        }

        [HttpPost]
        public async Task<IActionResult> ShortenUrl([FromBody] CreateUrlShorteningRequestDto requestDto)
        {
            var validationResult = await _validator.ValidateAsync(requestDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.FirstOrDefault().ErrorMessage);


            if(Uri.TryCreate(requestDto.OriginalUrl, UriKind.Absolute, out var _))
            {
                var result = await _urlShorteningService.CreateUrlShorteningAsync(requestDto);

                if (result.IsExists)
                {
                    return Conflict("Given original url is already recorded.");
                }

                return Ok(result);
            }

            return BadRequest("Invalid URL format. Try to start with valid domain form such as 'https://sapmle-site.com'");
        }
    }
}
