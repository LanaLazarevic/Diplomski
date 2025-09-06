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

namespace PFM.Application.UseCases.Cards.Commands.CreateCard
{
    public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, OperationResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CreateCardDto> _validator;

        public CreateCardCommandHandler(IUnitOfWork uow, IValidator<CreateCardDto> validator)
        {
            _uow = uow;
            _validator = validator;
        }

        public async Task<OperationResult> Handle(CreateCardCommand request, CancellationToken cancellationToken)
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

                var account = await _uow.Accounts.GetByNumber(dto.AccountNumber, cancellationToken);
                if (account == null || account.UserId != user.Id)
                {
                    var error = new BusinessError
                    {
                        Problem = "account-number",
                        Message = "Account not found or does not belong to user",
                        Details = $"Account with number {dto.AccountNumber} does not exist or does not belong to user with jmbg {dto.UserJmbg}"
                    };
                    return OperationResult.Fail(440, new[] { error });
                }

                var cardType = Enum.Parse<CardTypeEnum>(dto.CardType, true);
                var card = new Card
                {
                    Id = Guid.NewGuid(),
                    OwnerName = dto.OwnerName,
                    CardNumber = dto.CardNumber,
                    ExpirationDate = dto.ExpirationDate,
                    UserId = user.Id,
                    CardType = cardType,
                    IsActive = true,
                    AccountId = account.Id
                };

                _uow.Cards.Add(card);
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
