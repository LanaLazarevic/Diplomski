using FluentValidation;
using PFM.Application.Dto;
using PFM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.Validation
{
    public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountDtoValidator()
        {
            RuleFor(x => x.AccountNumber)
                .GreaterThan(0).WithMessage("account-number:invalid-value:account-number must be greater than zero");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("currency:required:currency is required")
                .Length(3).WithMessage("currency:invalid-length:currency must be 3 characters long");

            RuleFor(x => x.UserJmbg)
                .NotEmpty().WithMessage("user-jmbg:required:user-jmbg is required")
                .Length(13).WithMessage("user-jmbg:invalid-length:user-jmbg must be 13 characters long");

            RuleFor(x => x.AvailableAmount)
                .GreaterThanOrEqualTo(0).WithMessage("available-amount:invalid-value:available-amount must be non-negative");

            RuleFor(x => x.ReservedAmount)
                .GreaterThanOrEqualTo(0).WithMessage("reserved-amount:invalid-value:reserved-amount must be non-negative");

            RuleFor(x => x.AccountType)
                .Must(t => Enum.TryParse<AccountTypeEnum>(t, true, out _))
                .WithMessage(ctx =>
                {
                    var names = string.Join(", ", Enum.GetNames(typeof(AccountTypeEnum)));
                    return $"account-type:unknown-enum:account-type must be one of: {names}";
                });
        }
    }
}
