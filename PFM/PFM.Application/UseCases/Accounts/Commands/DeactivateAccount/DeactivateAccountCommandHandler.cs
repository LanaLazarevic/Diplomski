using MediatR;
using PFM.Application.Result;
using PFM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Accounts.Commands.DeactivateAccount
{
    public class DeactivateAccountCommandHandler : IRequestHandler<DeactivateAccountCommand, OperationResult>
    {
        private readonly IUnitOfWork _uow;

        public DeactivateAccountCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<OperationResult> Handle(DeactivateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _uow.Accounts.GetByIdAsync(request.Id, cancellationToken);
                if (account == null)
                {
                    var error = new BusinessError
                    {
                        Problem = "account-id",
                        Message = "Account not found",
                        Details = $"Account with id {request.Id} does not exist"
                    };
                    return OperationResult.Fail(440, new[] { error });
                }

                if (!account.IsActive)
                {
                    var error = new BusinessError
                    {
                        Problem = "account-active",
                        Message = "Account already inactive",
                        Details = "Account is already inactive"
                    };
                    return OperationResult.Fail(440, new[] { error });
                }

                account.IsActive = false;
                _uow.Accounts.Update(account);
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
