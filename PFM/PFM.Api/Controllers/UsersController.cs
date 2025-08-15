using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PFM.Application.Dto;
using PFM.Application.Result;
using PFM.Application.UseCases.Users.Commands;

namespace PFM.Api.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto request)
        {
            var cmd = new CreateUserCommand(request);
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
                else
                {
                    errors = op.Error!
                        .OfType<ServerError>()
                        .Select(e => new { message = e.Message })
                        .ToList();
                    return StatusCode(op.code, errors);
                }
            }

            return StatusCode(op.code, "User successfully added");
        }
    }
}
