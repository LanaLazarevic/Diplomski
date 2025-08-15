using MediatR;
using PFM.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Users.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<OperationResult<LoginResult>>;
}
