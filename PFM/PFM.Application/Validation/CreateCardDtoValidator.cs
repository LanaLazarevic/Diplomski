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
    public class CreateCardDtoValidator : AbstractValidator<CreateCardDto>
    {
        public CreateCardDtoValidator()
        {
            RuleFor(x => x.OwnerName)
                .NotEmpty().WithMessage("owner-name:required:owner-name is required");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("card-number:required:card-number is required");

            RuleFor(x => x.ExpirationDate)
                .Must(d => d > DateOnly.Parse(DateTime.UtcNow.ToShortDateString().ToString()))
                .WithMessage("expiration-date:invalid-value:expiration-date must be in the future");

            RuleFor(x => x.UserJmbg)
                .NotEmpty().WithMessage("user-jmbg:required:user-jmbg is required")
                .Length(13).WithMessage("jmbg:invalid-length:jmbg must be 13 characters long");



            RuleFor(x => x.CardType)
                .Must(t => Enum.TryParse<CardTypeEnum>(t, true, out _))
                .WithMessage(ctx =>
                {
                    var names = string.Join(", ", Enum.GetNames(typeof(CardTypeEnum)));
                    return $"card-type:unknown-enum:card-type must be one of: {names}";
                });
        }
    }
}
