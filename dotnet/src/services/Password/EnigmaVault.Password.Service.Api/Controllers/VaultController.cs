using EnigmaVault.Password.Service.Api.Extensions;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.AddTag;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.AddToFavorites;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.Archive;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.Create;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.Delete;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.EmptyTrash;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.MoveToTrash;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.RemoveFromFavorites;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.RemoveTag;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.RestoreAllFromTrash;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.RestoreFromTrash;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.UnArchive;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.Update;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Queries.GetAll;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql.Replication;
using Shared.Contracts.Requests.PasswordService;

namespace EnigmaVault.Password.Service.Api.Controllers
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
                     request.EncryptedOverview,
                     request.EncryptedDetails);

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
                request.EncryptedOverview,
                request.EncryptedDetails);

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

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var query = new GetVaultByIdQuery(id, extractResult.Value.UserId);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

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

            var command = new RemoveTagFromVaultItemCommand(extractResult.Value.UserId, vaultId, tagId);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }
    }
}