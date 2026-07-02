using FluentValidation;

namespace EnigmaVault.Password.Service.Application.Features.Validators
{
    public sealed class MustUserIdValidator : AbstractValidator<IMustHasUserId>
    {
        public MustUserIdValidator() => RuleFor(x => x.UserId).NotEmpty().WithMessage("Идентификатор пользователя не может быть пустым.");
    }
}