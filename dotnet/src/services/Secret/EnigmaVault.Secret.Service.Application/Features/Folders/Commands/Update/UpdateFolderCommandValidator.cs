using EnigmaVault.Secret.Service.Application.Features.Folders.Validators;
using EnigmaVault.Secret.Service.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.Secret.Service.Application.Features.Folders.Commands.Update
{
    public sealed class UpdateFolderCommandValidator : AbstractValidator<UpdateFolderCommand>
    {
        public UpdateFolderCommandValidator()
        {
            Include(new GuidValidator());
            Include(new NameFolderValidator());
            Include(new MustUserIdValidator());
        }
    }
}