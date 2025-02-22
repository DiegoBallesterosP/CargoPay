using CargoPay.Dtos;
using FluentValidation;

namespace CargoPay.Validators
{
    public class PaymentDtoValidator : AbstractValidator<PaymentDto>
    {
        public PaymentDtoValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("The card number cannot be empty.")
                .Length(15).WithMessage("The card number must be 15 digits long.")
                .Matches(@"^\d{15}$").WithMessage("The card number must contain only digits.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("The transaction amount must be greater than zero.");
        }
    }
}