using FluentValidation;
using PFM.Application.Dto;
using PFM.Domain.Enums;

namespace PFM.Application.Validation
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("first-name:required:first-name is required")
                .Must(x => !int.TryParse(x, out _)).WithMessage("first-name:invalid-type:first-name must be a string");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("last-name:required:last-name is required")
                .Must(x => !int.TryParse(x, out _)).WithMessage("last-name:invalid-type:last-name must be a string");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("email:required:email is required")
                .EmailAddress().WithMessage("email:invalid-format:email must be a valid email address");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("password:required:password is required");

            RuleFor(x => x.PhoneNumber)
                .Matches("^\\+?[0-9]*$")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("phone-number:invalid-format:phone-number must contain only digits");

            RuleFor(x => x.Birthday)
                .Must(b => b <= DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18)))
                .WithMessage("birthday:invalid-value:user must be older than 18");

            RuleFor(x => x.Role)
                .Must(r => Enum.TryParse<RoleEnum>(r, true, out _))
                .WithMessage(ctx =>
                {
                    var names = string.Join(", ", Enum.GetNames(typeof(RoleEnum)));
                    return $"role:unknown-enum:role must be one of: {names}";
                });

            RuleFor(x => x.Jmbg)
                .NotEmpty().WithMessage("jmbg:required:jmbg is required")
                .Length(13).WithMessage("jmbg:invalid-length:jmbg must be 13 characters long");
        }
    }
}
