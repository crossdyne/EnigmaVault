using FluentValidation;

namespace EnigmaVault.Secret.Service.Application.Features.Validators
{
    public sealed class SvgCodeValidator : AbstractValidator<IHasSvgCode>
    {
        public SvgCodeValidator()
            => RuleFor(x => x.SvgCode).NotEmpty().WithMessage("SVG code не может быть пустым.");
    }
}