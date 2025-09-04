using FluentValidation;
using MediatR;
using PFM.Application.Dto;
using PFM.Application.Result;
using PFM.Domain.Entities;
using PFM.Domain.Enums;
using PFM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, OperationResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CreateAccountDto> _validator;

        public CreateAccountCommandHandler(IUnitOfWork uow, IValidator<CreateAccountDto> validator)
        {
            _uow = uow;
            _validator = validator;
        }

        public async Task<OperationResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var validation = await _validator.ValidateAsync(dto, cancellationToken);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e =>
                {
                    var raw = e.ErrorMessage ?? string.Empty;
                    var parts = raw.Split(':');
                    var tag = parts.ElementAtOrDefault(0) ?? e.PropertyName;
                    var code = parts.ElementAtOrDefault(1) ?? e.ErrorCode;
                    var message = parts.ElementAtOrDefault(2) ?? raw;
                    return new ValidationError
                    {
                        Tag = tag,
                        Error = code,
                        Message = message
                    };
                }).ToList();
                return OperationResult.Fail(400, errors);
            }
            try
            {
                var user = await _uow.Users.GetByJmbg(dto.UserJmbg, cancellationToken);
                if (user == null)
                {
                    var error = new BusinessError
                    {
                        Problem = "user-jmbg",
                        Message = "User not found",
                        Details = $"User with jmbg {dto.UserJmbg} does not exist"
                    };
                    return OperationResult.Fail(440, new[] { error });
                }

                var accountType = Enum.Parse<AccountTypeEnum>(dto.AccountType, true);
                var account = new Account
                {
                    Id = Guid.NewGuid(),
                    AccountNumber = dto.AccountNumber,
                    AvailableAmount = dto.AvailableAmount,
                    ReservedAmount = dto.ReservedAmount,
                    Currency = dto.Currency,
                    AccountType = accountType,
                    IsActive = true,
                    UserId = user.Id
                };

                _uow.Accounts.Add(account);
                await _uow.SaveChangesAsync(cancellationToken);

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                var problem = new ServerError { Message = ex.Message };
                List<ServerError> errors = new() { problem };
                return OperationResult.Fail(503, errors);
            }
        }
    }
}
