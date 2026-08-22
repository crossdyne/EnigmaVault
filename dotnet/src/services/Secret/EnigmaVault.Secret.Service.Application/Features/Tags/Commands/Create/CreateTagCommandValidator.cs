using EnigmaVault.Secret.Service.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.Secret.Service.Application.Features.Tags.Commands.Create
{
    public sealed class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            Include(new MustUserIdValidator());
            Include(new TagNameValidator());
        }
    }
}