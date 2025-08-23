using FluentValidation;
using PFM.Application.Dto;
using PFM.Domain.Enums;

namespace PFM.Application.Validation
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .Must(x => !int.TryParse(x, out _)).WithMessage("first-name:invalid-type:first-name must be a string");

            RuleFor(x => x.LastName)
                .Must(x => !int.TryParse(x, out _)).WithMessage("last-name:invalid-type:last-name must be a string");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("email:invalid-format:email must be a valid email address");

            RuleFor(x => x.PhoneNumber)
                .Matches("^\\+?[0-9]*$")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("phone-number:invalid-format:phone-number must contain only digits");

        }
    }
}
