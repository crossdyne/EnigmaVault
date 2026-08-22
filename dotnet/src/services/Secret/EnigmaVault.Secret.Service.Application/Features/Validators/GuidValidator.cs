using FluentValidation;

namespace EnigmaVault.Secret.Service.Application.Features.Validators
{
    public sealed class GuidValidator : AbstractValidator<IHasGuidId>
    {
        public GuidValidator() => RuleFor(x => x.Id).NotEmpty().WithMessage("Идентификатор записи не может быть пустым.");
    }
}