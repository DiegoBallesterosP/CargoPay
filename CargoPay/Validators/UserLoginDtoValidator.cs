using CargoPay.Dtos;
using FluentValidation;

namespace CargoPay.Validators
{
    public class UserLoginDtoValidator : AbstractValidator<LoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username cannot be empty.")
                .Length(3, 30).WithMessage("Username must be between 3 and 30 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
