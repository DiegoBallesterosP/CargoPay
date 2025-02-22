using CargoPay.Dtos;
using FluentValidation;

namespace CargoPay.Validators
{
    public class CardDtoValidator : AbstractValidator<CardDto>
    {
        public CardDtoValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("The card number cannot be empty.")
                .Length(15).WithMessage("The card number must be 15 digits long.")
                .Matches(@"^\d{15}$").WithMessage("The card number must contain only digits.");

            RuleFor(x => x.Balance)
                .GreaterThan(0).WithMessage("The initial balance must be greater than zero.");
        }
    }
}