using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.AddTag;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.AddToFavorites;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Archive;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Create;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Delete;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.EmptyTrash;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.MoveToTrash;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RemoveFromFavorites;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RemoveTag;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RestoreAllFromTrash;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RestoreFromTrash;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.UnArchive;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Update;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Queries.GetAll;
using EnigmaVault.PasswordService.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests.PasswordService;

namespace EnigmaVault.PasswordService.Controllers
{
    [ApiController]
    [Route("api/vault")]
    public class VaultController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /*--Create----------------------------------------------------------------------------------------*/

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateVaultItemRequest request)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new CreateVaultItemCommand(
                     extractResult.Value.UserId,
                     request.PasswordType,
                     Guid.Parse(request.IconId),
                     Convert.FromBase64String(request.EncryptedOverview),
                     Convert.FromBase64String(request.EncryptedDetails));

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok(result.Value);
        }

        /*--Update----------------------------------------------------------------------------------------*/

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateVaultItemRequest request)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new UpdateVaultItemCommand(
                extractResult.Value.UserId,
                Guid.Parse(request.VaultItemId),
                Guid.Parse(request.IconId),
                Convert.FromBase64String(request.EncryptedOverview),
                Convert.FromBase64String(request.EncryptedDetails));

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok(result.Value);
        }

        [HttpPatch("add-favorites/{vaultId}")]
        [Authorize]
        public async Task<IActionResult> AddToFavorites([FromRoute] Guid vaultId)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new AddToFavoritesVaultCommand(vaultId, extractResult.Value.UserId);

            var result = await _mediator.Send(command);

           if (result.IsFailure)
               return BadRequest(result.StringMessage);
            
            return Ok();
        }

        [HttpPatch("remove-favorites/{vaultId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromFavorites([FromRoute] Guid vaultId)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new RemoveFromFavoritesVaultCommand(vaultId, extractResult.Value.UserId);

            var result = await _mediator.Send(command);

           if (result.IsFailure)
               return BadRequest(result.StringMessage);
            
            return Ok();
        }

        [HttpPatch("archive/{vaultId}")]
        [Authorize]
        public async Task<IActionResult> Archive([FromRoute] Guid vaultId)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new ArchiveVaultCommand(vaultId, extractResult.Value.UserId);

            var result = await _mediator.Send(command);

           if (result.IsFailure)
               return BadRequest(result.StringMessage);
            
            return Ok();
        }

        [HttpPatch("un-archive/{vaultId}")]
        [Authorize]
        public async Task<IActionResult> UnArchive([FromRoute] Guid vaultId)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new UnArchiveVaultCommand(vaultId, extractResult.Value.UserId);

            var result = await _mediator.Send(command);

           if (result.IsFailure)
               return BadRequest(result.StringMessage);
            
            return Ok();
        }

        /*--Delete----------------------------------------------------------------------------------------*/

        [HttpDelete("{vaultId}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid vaultId)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new DeleteVaultItemCommand(extractResult.Value.UserId, vaultId);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        [HttpPatch("empty-trash")]
        [Authorize]
        public async Task<IActionResult> EmptyTrash()
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new EmptyVaultsTrashCommand(extractResult.Value.UserId);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        [HttpPatch("move-to-trash/{vaultId}")]
        [Authorize]
        public async Task<IActionResult> MoveToTrash(Guid vaultId)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new MoveVaultToTrashCommand(extractResult.Value.UserId, vaultId);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok(result.Value);
        }

        [HttpPatch("restore-from-trash/{vaultId}")]
        [Authorize]
        public async Task<IActionResult> RestoreFromTrash(Guid vaultId)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new RestoreVaultFromTrashCommand(extractResult.Value.UserId, vaultId);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        [HttpPatch("restore-all-from-trash")]
        [Authorize]
        public async Task<IActionResult> RestoreAllFromTrash()
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new RestoreAllVaultsFromTrashCommand(extractResult.Value.UserId);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        /*--Get-------------------------------------------------------------------------------------------*/

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromRoute] Guid userId)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new GetAllVaultsQuery(extractResult.Value.UserId));

            return Ok(result.Value);
        }

        /*--Tags------------------------------------------------------------------------------------------*/

        [HttpPatch("add-tag/{vaultId}/{tagId}")]
        [Authorize]
        public async Task<IActionResult> AddTag([FromRoute] Guid vaultId, [FromRoute] Guid tagId)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new AddTagToVaultItemCommand(extractResult.Value.UserId, vaultId, tagId);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        [HttpPatch("remove-tag/{vaultId}/{tagId}")]
        [Authorize]
        public async Task<IActionResult> RemoveTag([FromRoute] Guid vaultId, [FromRoute] Guid tagId)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new RemoveTagFromVaulItemCommand(extractResult.Value.UserId, vaultId, tagId);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }
    }
}