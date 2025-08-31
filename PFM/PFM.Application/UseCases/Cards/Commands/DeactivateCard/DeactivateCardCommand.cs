using MediatR;
using PFM.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Cards.Commands.DeactivateCard
{
    public record DeactivateCardCommand(Guid Id) : IRequest<OperationResult>;
}
