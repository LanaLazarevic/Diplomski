using MediatR;
using PFM.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Accounts.Commands.DeactivateAccount
{
    public record DeactivateAccountCommand(Guid Id) : IRequest<OperationResult>;

}
