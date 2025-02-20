using CargoPay.Dtos;
using FluentValidation;

namespace CargoPay.Validators
{
    public class CardDtoValidator : AbstractValidator<CardDto>
    {
        public CardDtoValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("El número de tarjeta no puede estar vacío.")
                .Length(15).WithMessage("El número de tarjeta debe tener 15 dígitos.")
                .Matches(@"^\d{15}$").WithMessage("El número de tarjeta debe contener solo dígitos.");

            RuleFor(x => x.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("El saldo inicial no puede ser negativo.");
        }
    }
}
