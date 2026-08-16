using EnigmaVault.Password.Service.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.Password.Service.Application.Features.Tags.Commands.Update
{
    public sealed class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
    {
        public UpdateTagCommandValidator() => Include(new TagNameValidator());
    }
}