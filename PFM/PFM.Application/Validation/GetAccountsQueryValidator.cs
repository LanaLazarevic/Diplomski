using FluentValidation;
using PFM.Application.UseCases.Accounts.Queries.GetAll;
using PFM.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.Validation
{
    public class GetAccountsQueryValidator : AbstractValidator<GetAccountsQuery>
    {
        public GetAccountsQueryValidator()
        {
            RuleFor(x => x.AccountNumber)
                .GreaterThan(0)
                .When(x => x.AccountNumber.HasValue)
                .WithMessage("account-number:invalid-value:account-number must be greater than zero");

            RuleFor(x => x.UserJmbg)
                .Length(13)
                .WithMessage("user-jmbg:invalid-length:user-jmbg must be 13 characters long")
                .When(x => !string.IsNullOrEmpty(x.UserJmbg));

            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1)
                .WithMessage("page:out-of-range:page must be at least 1");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 1000)
                .WithMessage("page-size:out-of-range:page-size must be between 1 and 1000");

            RuleFor(x => x.SortOrder)
                .Must(s => Enum.TryParse<SortOrder>(s, true, out _))
                .WithMessage(ctx => {
                    var names = string.Join(", ", Enum.GetNames(typeof(SortOrder)));
                    return $"sort-order:unknown-enum:sort-order must be one of: {names}";
                });
        }
    }
}
