using CargoPay.Dtos;
using FluentValidation;

namespace CargoPay.Validators
{
    public class PaymentDtoValidator : AbstractValidator<PaymentDto>
    {
        public PaymentDtoValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("El número de tarjeta no puede estar vacío.")
                .Length(15).WithMessage("El número de tarjeta debe tener 15 dígitos.")
                .Matches(@"^\d{15}$").WithMessage("El número de tarjeta debe contener solo dígitos.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("El monto de la transacción debe ser mayor a cero.")
                .GreaterThanOrEqualTo(0).WithMessage("El monto de la transacción no puede ser negativo.");
        }
    }
}
