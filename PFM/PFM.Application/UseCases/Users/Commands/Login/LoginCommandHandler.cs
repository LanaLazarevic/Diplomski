using MediatR;
using PFM.Application.Interfaces;
using PFM.Application.Result;
using PFM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<LoginResult>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IUnitOfWork uow, ITokenService tokenService)
        {
            _uow = uow;
            _tokenService = tokenService;
        }

        public async Task<OperationResult<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _uow.Users.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                var error = new BusinessError
                {
                    Problem = "auth",
                    Message = "Invalid credentials",
                    Details = "Email or password is incorrect"
                };
                return OperationResult<LoginResult>.Fail(440, new[] { error });
            }

            var token = _tokenService.GenerateToken(user);
            var roles = new[] { user.Role.ToString() };
            var result = new LoginResult()
            {
                Jwt = token,
            };
            return OperationResult<LoginResult>.Success(result, 200);
        }
    }
}

