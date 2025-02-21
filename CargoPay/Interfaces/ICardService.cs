using CargoPay.Dtos;
using CargoPay.Entities;

namespace CargoPay.Interfaces
{
    public interface ICardService
    {
        Task<Card> CreateCard(string cardNumber, decimal balance);
        Task<bool> CardExists(string cardNumber);
        Task<PaymentResultDto> ProcessPayment(string cardNumber, decimal amount);
        Task<decimal> GetCardBalance(string cardNumber);
    }
}