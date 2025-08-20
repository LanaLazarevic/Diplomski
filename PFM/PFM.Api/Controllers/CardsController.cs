using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFM.Application.Dto;
using PFM.Application.Result;
using PFM.Application.UseCases.Cards.Commands.CreateCard;
using PFM.Application.UseCases.Cards.Queries.GetAll;
using PFM.Domain.Enums;
using System.Security.Claims;

namespace PFM.Api.Controllers
{
    [Route("cards")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CardsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = nameof(RoleEnum.admin))]
        public async Task<IActionResult> Create([FromBody] CreateCardDto request)
        {
            var cmd = new CreateCardCommand(request);
            var op = await _mediator.Send(cmd);

            if (!op.IsSuccess)
            {
                object? errors = null;
                if (op.code == 400)
                {
                    errors = op.Error!
                        .OfType<ValidationError>()
                        .Select(e => new
                        {
                            tag = e.Tag,
                            error = e.Error,
                            message = e.Message
                        })
                        .ToList();
                    return StatusCode(op.code, new { errors });
                }
                else if (op.code == 503)
                {
                    errors = op.Error!
                        .OfType<ServerError>()
                        .Select(e => new { message = e.Message })
                        .ToList();
                    return StatusCode(op.code, errors);
                } else
                {
                    errors = op.Error!
                        .OfType<BusinessError>()
                        .Select(e => new
                        {
                            problem = e.Problem,
                            message = e.Message,
                            details = e.Details
                        })
                        .FirstOrDefault();
                    return StatusCode(op.code, errors);
                }
            }

            return StatusCode(op.code, "Card successfully added");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] GetCardsQuery request)
        {
            if (!User.IsInRole(nameof(RoleEnum.admin)))
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdClaim, out var userId))
                {
                    request.UserId = userId;
                }
            }

            var op = await _mediator.Send(request);
            if (!op.IsSuccess)
            {
                var errors = op.Error!
                    .OfType<ServerError>()
                    .Select(e => new { message = e.Message })
                    .ToList();
                return StatusCode(op.code, errors);
            }

            return StatusCode(op.code, op.Value);
        }
    }
}
