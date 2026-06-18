using EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.CreateCommon;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.CreatePersonal;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.DeleteCommon;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.DeletePersonal;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.UpdateCommon;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.UpdatePersonal;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Queries.GetAll;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Queries.GetPersonal;
using EnigmaVault.PasswordService.Extentions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests.PasswordService;

namespace EnigmaVault.PasswordService.Controllers
{
    [ApiController]
    [Route("api/icon-categories")]
    public sealed class IconCategoryController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /*--Create----------------------------------------------------------------------------------------*/
      
        [HttpPost("common")]
        [Authorize(Roles = "Admin,SuperAdmin,Moderator")]
        public async Task<IActionResult> CreateCommon([FromBody] CreateIconCategoryCommonRequest request)
        {
            var result = await _mediator.Send(new CreateCommonIconCategoryCommand(request.Name));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpPost("personal")]
        [Authorize]
        public async Task<IActionResult> CreatePersonal([FromBody] CreateIconCategoryPersonalRequest request)
        {
            var extractResult = this.ExtactCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new CreatePersonalCategoryCommand(request.Name, extractResult.Value.UserId));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(result.Value),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Update----------------------------------------------------------------------------------------*/

        [HttpPatch("common")]
        [Authorize(Roles = "Admin,SuperAdmin,Moderator")]
        public async Task<IActionResult> UpdateCommon([FromBody] UpdateCommonIconCategoryRequest request)
        {
            var result = await _mediator.Send(new UpdateCommonIconCategoryCommand(request.Id, request.Name));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpPatch("personal")]
        [Authorize]
        public async Task<IActionResult> UpdatePersonal([FromBody] UpdatePersonalIconCategoryRequest request)
        {
            var extractResult = this.ExtactCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new UpdatePersonalIconCategoryCommand(request.Id, extractResult.Value.UserId, request.Name));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Delete----------------------------------------------------------------------------------------*/

        [HttpDelete("common/{id}")]
        [Authorize(Roles = "Admin,SuperAdmin,Moderator")]
        public async Task<IActionResult> DeleteCommon([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteCommonIconCategoryCommand(id));

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

            var result = await _mediator.Send(new DeletePersonalIconCategoryCommand(id, extractResult.Value.UserId));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Get-------------------------------------------------------------------------------------------*/

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var extractResult = this.ExtactCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new GetAllIconCategoriesQuery(extractResult.Value.UserId));

            return Ok(result);
        }

        [HttpGet("personal")]
        [Authorize]
        public async Task<IActionResult> GetAllPersonal()
        {
            var extractResult = this.ExtactCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new GetAllPersonalIconCategories(extractResult.Value.UserId));

            return Ok(result);
        }
    }
}