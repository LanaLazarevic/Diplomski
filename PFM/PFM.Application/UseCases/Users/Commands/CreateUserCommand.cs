using MediatR;
using PFM.Application.Dto;
using PFM.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Users.Commands
{
    public record CreateUserCommand(CreateUserDto Dto) : IRequest<OperationResult>;
}
