using EnigmaVault.Password.Service.Api.Extensions;
using EnigmaVault.Password.Service.Application.Features.Tags.Commands.Create;
using EnigmaVault.Password.Service.Application.Features.Tags.Commands.Delete;
using EnigmaVault.Password.Service.Application.Features.Tags.Commands.Update;
using EnigmaVault.Password.Service.Application.Features.Tags.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests.PasswordService;

namespace EnigmaVault.Password.Service.Api.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class TagController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /*--Create----------------------------------------------------------------------------------------*/

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new CreateTagCommand(extractResult.Value.UserId, request.Name, request.Color));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(result.Value),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Update----------------------------------------------------------------------------------------*/

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateTagRequest request)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new UpdateTagCommand(Guid.Parse(request.Id), extractResult.Value.UserId, request.Name, request.Color));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Delete----------------------------------------------------------------------------------------*/

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new DeleteTagCommand(id, extractResult.Value.UserId));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }
        /*--Get-------------------------------------------------------------------------------------------*/

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new GetAllTagsQuery(extractResult.Value.UserId));

            return Ok(result);
        }
    }
}