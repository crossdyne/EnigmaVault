using EnigmaVault.PasswordService.Application.Features.Icons.Commands.CreateCommon;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.CreatePersonal;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.DeleteCommon;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.DeletePersonal;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.UpdateCommon;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.UpdatePersonal;
using EnigmaVault.PasswordService.Application.Features.Icons.Queries.GetAll;
using EnigmaVault.PasswordService.Application.Features.Icons.Queries.GetPersonal;
using EnigmaVault.PasswordService.Extentions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests.PasswordService;
using System.Security.Claims;

namespace EnigmaVault.PasswordService.Controllers
{
    [ApiController]
    [Route("api/icons")]
    public sealed class IconController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /*--Create----------------------------------------------------------------------------------------*/

        [HttpPost("common")]
        [Authorize(Roles = "Admin,SuperAdmin,Moderator")]
        public async Task<IActionResult> CreateCommon([FromBody] CreateIconCommonRequest request)
        {
            var result = await _mediator.Send(new CreateCommonIconCommand(request.SvgCode, request.Name, Guid.Parse(request.IconCategoryId)));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpPost("personal")]
        [Authorize]
        public async Task<IActionResult> CreatePersonal([FromBody] CreateIconPersonalRequest request)
        {
            var extractResult = this.ExtactCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new CreatePersonalIconCommand(extractResult.Value.UserId, request.SvgCode,request.Name, Guid.Parse(request.IconCategoryId)));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(result.Value),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Update----------------------------------------------------------------------------------------*/

        [HttpPatch("common")]
        [Authorize(Roles = "Admin,SuperAdmin,Moderator")]
        public async Task<IActionResult> UpdateCommon([FromBody] UpdateCommonIconRequest request)
        {
            var result = await _mediator.Send(new UpdateCommonIconCommand(Guid.Parse(request.Id), request.Name, request.SvgCode, Guid.Parse(request.IconCategoryId)));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpPatch("personal")]
        [Authorize]
        public async Task<IActionResult> UpdatePersonal([FromBody] UpdatePersonalIconRequest request)
        {
            var extractResult = this.ExtactCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new UpdatePersonalIconCommand(Guid.Parse(request.Id), extractResult.Value.UserId, request.Name, request.SvgCode, Guid.Parse(request.IconCategoryId)));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Delete----------------------------------------------------------------------------------------*/

        [HttpDelete("common/{id}")]
        [Authorize(Roles = "Admin,SuperAdmin,Moderator")]
        public async Task<IActionResult> DeleteCommon([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteCommonIconCommand(id));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpDelete("personal/{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePersonal([FromRoute] Guid id)
        {
            var extractResult = this.ExtactCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new DeletePersonalIconCommand(id, extractResult.Value.UserId));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Get-------------------------------------------------------------------------------------------*/

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var extractResult = this.ExtactCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new GetAllIconQuery(extractResult.Value.UserId));

            return Ok(result);
        }

        [HttpGet("personal")]
        [Authorize]
        public async Task<IActionResult> GetAllPersonal()
        {
            var extractResult = this.ExtactCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new GetAllPersonalIconQuery(extractResult.Value.UserId));

            return Ok(result);
        }
    }
}