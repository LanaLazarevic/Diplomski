using MediatR;
using Microsoft.AspNetCore.Mvc;
using PFM.Application.Dto;
using PFM.Application.Result;
using PFM.Application.UseCases.Cards.Commands.CreateCard;

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
    }
}
