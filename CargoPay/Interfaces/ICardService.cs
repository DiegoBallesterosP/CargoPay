using CargoPay.Entities;

namespace CargoPay.Interfaces
{
    public interface ICardService
    {
        Task<Card> CreateCard(string cardNumber, decimal balance);
        Task<bool> CardExists(string cardNumber);
        Task<bool> ProcessPayment(string cardNumber, decimal amount);
        Task<decimal> GetCardBalance(string cardNumber);

    }
}