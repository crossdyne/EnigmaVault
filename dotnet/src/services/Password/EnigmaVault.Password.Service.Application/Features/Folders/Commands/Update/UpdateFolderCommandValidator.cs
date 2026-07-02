using EnigmaVault.Password.Service.Application.Features.Folders.Validators;
using EnigmaVault.Password.Service.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.Password.Service.Application.Features.Folders.Commands.Update
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