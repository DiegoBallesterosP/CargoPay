using Microsoft.AspNetCore.Mvc;
using CargoPay.Dtos;
using CargoPay.Interfaces;
using FluentValidation;
using CargoPay.Entities;
using Microsoft.AspNetCore.Authorization;

namespace CargoPay.Controllers
{
    [ApiController]
    [Route("api/cards")]
    [Authorize]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IValidator<CardDto> _cardValidator;
        private readonly IValidator<PaymentDto> _paymentValidator;

        public CardController(ICardService cardService, IValidator<CardDto> cardValidator, IValidator<PaymentDto> paymentValidator)
        {
            _cardService = cardService;
            _cardValidator = cardValidator;
            _paymentValidator = paymentValidator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCards([FromBody] List<CardDto> requests)
        {
            var results = new List<Card>();
            foreach (var request in requests)
            {
                var validationResult = await _cardValidator.ValidateAsync(request);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                try
                {
                    var card = await _cardService.CreateCard(request.CardNumber, request.Balance);
                    results.Add(card);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al crear la tarjeta: {ex.Message}");
                }
            }
            return Ok(results);
        }

        [HttpPost("pay")]
        public async Task<IActionResult> ProcessPayments([FromBody] List<PaymentDto> requests)
        {
            var results = new List<PaymentResultDto>();
            foreach (var request in requests)
            {
                var validationResult = await _paymentValidator.ValidateAsync(request);
                if (!validationResult.IsValid)                
                    return BadRequest(validationResult.Errors);
                

                try
                {
                    var result = await _cardService.ProcessPayment(request.CardNumber, request.Amount);
                    results.Add(result);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al procesar el pago: {ex.Message}");
                }
            }
            return Ok(results);
        }

        [HttpPost("balance")]
        public async Task<IActionResult> GetCardBalances([FromBody] List<string> cardNumbers)
        {
            var results = new List<object>();
            foreach (var cardNumber in cardNumbers)
            {
                try
                {
                    var balance = await _cardService.GetCardBalance(cardNumber);
                    results.Add(new
                    {
                        CardNumber = cardNumber,
                        Balance = balance
                    });
                }
                catch (InvalidOperationException)
                {
                    results.Add(new
                    {
                        CardNumber = cardNumber,
                        Balance = "La tarjeta no existe."
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al obtener el saldo de la tarjeta: {ex.Message}");
                }
            }
            return Ok(results);
        }
    }
}