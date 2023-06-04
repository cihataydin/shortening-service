using FluentValidation;
using Shortening.API.Dtos.RequestDtos;

namespace Shortening.API.Validators
{
    public class CreateUrlShorteningValidator : AbstractValidator<CreateUrlShorteningRequestDto>
    {
        public CreateUrlShorteningValidator()
        {
            RuleFor(u => u.OriginalUrl).NotNull();
        }
    }
}
