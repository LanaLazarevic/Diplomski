using MediatR;
using PFM.Application.Dto;
using PFM.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Accounts.Commands.CreateAccount
{
    public record CreateAccountCommand(CreateAccountDto Dto) : IRequest<OperationResult>;
}
