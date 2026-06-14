using System.Security.Claims;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.CreateCommon;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.CreatePersonal;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.DeleteCommon;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.DeletePersonal;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.UpdateCommon;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.UpdatePersonal;
using EnigmaVault.PasswordService.Application.Features.Icons.Queries.GetAll;
using EnigmaVault.PasswordService.Application.Features.Icons.Queries.GetPersonal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests.PasswordService;

namespace EnigmaVault.PasswordService.Controllers
{
    [ApiController]
    [Route("api/icons")]
    public sealed class IconController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /*--Create----------------------------------------------------------------------------------------*/

        [HttpPost("common")]
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
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("User ID не найден в токене.");

            if (!Guid.TryParse(userIdString, out var userIdGuid))
                return BadRequest("Не верный User ID формат.");

            var result = await _mediator.Send(new CreatePersonalIconCommand(userIdGuid, request.SvgCode,request.Name, Guid.Parse(request.IconCategoryId)));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(result.Value),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Update----------------------------------------------------------------------------------------*/

        [HttpPatch("common")]
        public async Task<IActionResult> UpdateCommon([FromBody] UpdateCommonIconRequest request)
        {
            var result = await _mediator.Send(new UpdateCommonIconCommand(Guid.Parse(request.Id), request.Name, request.SvgCode, Guid.Parse(request.IconCategoryId)));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpPatch("personal")]
        public async Task<IActionResult> UpdatePersonal([FromBody] UpdatePersonalIconRequest request)
        {
            var result = await _mediator.Send(new UpdatePersonalIconCommand(Guid.Parse(request.Id), Guid.Parse(request.UserId), request.Name, request.SvgCode, Guid.Parse(request.IconCategoryId)));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Delete----------------------------------------------------------------------------------------*/

        [HttpDelete("common/{id}")]
        public async Task<IActionResult> DeleteCommon([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteCommonIconCommand(id));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpDelete("personal/{userId}/{id}")]
        public async Task<IActionResult> DeletePersonal([FromRoute] Guid id, [FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new DeletePersonalIconCommand(id, userId));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Get-------------------------------------------------------------------------------------------*/

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("User ID не найден в токене.");

            if (!Guid.TryParse(userIdString, out var userIdGuid))
                return BadRequest("Не верный User ID формат.");

            var result = await _mediator.Send(new GetAllIconQuery(userIdGuid));

            return Ok(result);
        }

        [HttpGet("personal")]
        [Authorize]
        public async Task<IActionResult> GetAllPersonal()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("User ID не найден в токене.");

            if (!Guid.TryParse(userIdString, out var userIdGuid))
                return BadRequest("Не верный User ID формат.");

            var result = await _mediator.Send(new GetAllPersonalIconQuery(userIdGuid));

            return Ok(result);
        }
    }
}