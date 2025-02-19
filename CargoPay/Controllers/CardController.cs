using CargoPay.Dtos;
using CargoPay.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace CargoPay.Controllers
{
    [ApiController]
    [Route("api/cards")]
    [Authorize]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCard([FromBody] CardDto request)
        {
            if (request == null)
                return BadRequest("La solicitud no puede estar vacía.");

            if (string.IsNullOrEmpty(request.CardNumber) || request.CardNumber.Length != 15)
                return BadRequest("El número de tarjeta debe tener 15 dígitos.");

            if (!Regex.IsMatch(request.CardNumber, @"^\d{15}$"))
                return BadRequest("El número de tarjeta debe contener solo dígitos.");

            if (request.Balance < 0)
                return BadRequest("El saldo inicial no puede ser negativo.");

            try
            {
                var card = await _cardService.CreateCard(request.CardNumber, request.Balance);
                return Ok(card);
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

        [HttpPost("pay")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDto request)
        {
            if (request == null)
                return BadRequest("La solicitud no puede estar vacía.");

            if (string.IsNullOrEmpty(request.CardNumber) || request.CardNumber.Length != 15)
                return BadRequest("El número de tarjeta debe tener 15 dígitos.");

            if (!Regex.IsMatch(request.CardNumber, @"^\d{15}$"))
                return BadRequest("El número de tarjeta debe contener solo dígitos.");

            if (request.Amount <= 0)
                return BadRequest("El monto de la transacción debe ser mayor a cero.");

            try
            {
                var result = await _cardService.ProcessPayment(request.CardNumber, request.Amount);
                return Ok(result);
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

        [HttpGet("balance/{cardNumber}")]
        public async Task<IActionResult> GetCardBalance(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length != 15)
                return BadRequest("El número de tarjeta debe tener 15 dígitos.");

            if (!Regex.IsMatch(cardNumber, @"^\d{15}$"))
                return BadRequest("El número de tarjeta debe contener solo dígitos.");

            try
            {
                var balance = await _cardService.GetCardBalance(cardNumber);
                return Ok(balance);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el saldo de la tarjeta: {ex.Message}");
            }
        }
    }
}


