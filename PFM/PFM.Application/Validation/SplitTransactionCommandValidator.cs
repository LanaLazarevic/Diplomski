using FluentValidation;
using PFM.Application.UseCases.Transaction.Commands.SplitTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.Validation
{
    public class SplitTransactionCommandValidator : AbstractValidator<SplitTransactionCommand>
    {
        public SplitTransactionCommandValidator()
        {
            RuleFor(c => c.Splits)
                .NotNull()
                .WithMessage("splits:required:At least two split items are required.")
                .Must(s => s != null && s.Count() >= 2)
                .WithMessage("splits:out-of-range:At least two split items are required.")
                .When(c => c.Splits != null);

            RuleForEach(c => c.Splits).ChildRules(split =>
            {
                split.RuleFor(s => s.CatCode)
                    .NotEmpty()
                    .OverridePropertyName("splits.catcode")
                    .WithMessage("catcode:required:catcode is required and must be a non-empty string.");

                split.RuleFor(s => s.Amount)
                    .GreaterThan(0)
                    .OverridePropertyName("splits.amount")
                    .WithMessage("amount:out-of-range:amount must be greater than 0.");
            });
        }
    }
}
