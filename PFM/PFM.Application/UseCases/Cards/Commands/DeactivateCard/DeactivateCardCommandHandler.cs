using MediatR;
using PFM.Application.Result;
using PFM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Cards.Commands.DeactivateCard
{
    public class DeactivateCardCommandHandler : IRequestHandler<DeactivateCardCommand, OperationResult>
    {
        private readonly IUnitOfWork _uow;

        public DeactivateCardCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<OperationResult> Handle(DeactivateCardCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var card = await _uow.Cards.GetByIdAsync(request.Id, cancellationToken);
                if (card == null)
                {
                    var error = new BusinessError
                    {
                        Problem = "card-id",
                        Message = "Card not found",
                        Details = $"Card with id {request.Id} does not exist"
                    };
                    return OperationResult.Fail(440, new[] { error });
                }

                if (!card.IsActive)
                {
                    var error = new BusinessError
                    {
                        Problem = "card-active",
                        Message = "Card already inactive",
                        Details = "Card is already inactive"
                    };
                    return OperationResult.Fail(440, new[] { error });
                }

                card.IsActive = false;
                _uow.Cards.Update(card);
                await _uow.SaveChangesAsync(cancellationToken);

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                var problem = new ServerError { Message = ex.Message };
                return OperationResult.Fail(503, new[] { problem });
            }
        }
    }
}
