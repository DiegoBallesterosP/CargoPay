using CargoPay.Entities;
using CargoPay.Infrastructure;
using CargoPay.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CargoPay.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly ApplicationDbContext _context;

        public CardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCard(Card card)
        {
            await _context.Cards.AddAsync(card).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> CardExists(string cardNumber)
        {
            return await _context.Cards.AnyAsync(c => c.CardNumber == cardNumber).ConfigureAwait(false);
        }

        public async Task<Card> GetByCardNumber(string cardNumber)
        {
            return await _context.Cards.FirstOrDefaultAsync(c => c.CardNumber == cardNumber).ConfigureAwait(false);
        }

        public async Task UpdateCard(Card card)
        {
            _context.Cards.Update(card);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}