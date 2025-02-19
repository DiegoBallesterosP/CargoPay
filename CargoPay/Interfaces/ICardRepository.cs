using CargoPay.Entities;

namespace CargoPay.Interfaces
{
    public interface ICardRepository
    {
        Task AddCard(Card card);
        Task<bool> CardExists(string cardNumber);
        Task<Card> GetByCardNumber(string cardNumber);
        Task UpdateCard(Card card);
    }
}
