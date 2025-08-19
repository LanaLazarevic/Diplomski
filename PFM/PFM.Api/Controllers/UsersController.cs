using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFM.Application.Dto;
using PFM.Application.Result;
using PFM.Application.UseCases.Users.Commands.CreateUser;
using PFM.Application.UseCases.Users.Commands.UpdateUser;
using PFM.Application.UseCases.Users.Queries.GetAll;
using PFM.Application.UseCases.Users.Queries.GetOne;
using PFM.Domain.Enums;

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
        [Authorize(Roles = nameof(RoleEnum.admin))]
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

        [HttpGet]
        [Authorize(Roles = nameof(RoleEnum.admin))]
        public async Task<IActionResult> GetAll([FromQuery] GetUsersQuery request)
        {
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

        [HttpGet("{id}")]
        [Authorize(Roles = nameof(RoleEnum.admin))]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var op = await _mediator.Send(query);
            if (!op.IsSuccess)
            {
                object? errors = null;
                if (op.code == 440)
                {
                    errors = op.Error!
                        .OfType<BusinessError>()
                        .Select(e => new { problem = e.Problem, message = e.Message, details = e.Details })
                        .FirstOrDefault();
                    return StatusCode(op.code, errors);
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

            return StatusCode(op.code, op.Value);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleEnum.admin))]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateUserDto request)
        {
            var cmd = new UpdateUserCommand(id, request);
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
                else if (op.code == 440)
                {
                    errors = op.Error!
                        .OfType<BusinessError>()
                        .Select(e => new { problem = e.Problem, message = e.Message, details = e.Details })
                        .FirstOrDefault();
                    return StatusCode(op.code, errors);
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

            return StatusCode(op.code, "User successfully updated");
        }
    }
}
