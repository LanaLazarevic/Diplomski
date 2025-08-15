using MediatR;
using Microsoft.AspNetCore.Mvc;
using PFM.Api.Reqest;
using PFM.Application.UseCases.Users.Commands.Login;

namespace PFM.Api.Controllers
{
    [ApiController]
    [Route("login")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _mediator.Send(new LoginCommand(request.Email, request.Password));
            if (!result.IsSuccess)
                return StatusCode(result.code, result.Error);

            return Ok(result.Value);
        }
    }
}
