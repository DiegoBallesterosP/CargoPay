using CargoPay.Entities;
using CargoPay.Interfaces;

namespace CargoPay.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IUniversalFeesExchangeService _feesService;

        public CardService(ICardRepository cardRepository, IUniversalFeesExchangeService feesService)
        {
            _cardRepository = cardRepository;
            _feesService = feesService;
        }

        public async Task<Card> CreateCard(string cardNumber, decimal balance)
        {
            if (await CardExists(cardNumber))
                throw new InvalidOperationException("Una tarjeta con este número ya existe.");


            var card = new Card
            {
                CardNumber = cardNumber,
                Balance = balance,
                IsActive = true
            };

            await _cardRepository.AddCard(card);
            return card;
        }

        public async Task<bool> CardExists(string cardNumber)
        {
            return await _cardRepository.CardExists(cardNumber);
        }

        public async Task<bool> ProcessPayment(string cardNumber, decimal amount)
        {
            var card = await _cardRepository.GetByCardNumber(cardNumber);

            if (card == null)
                throw new InvalidOperationException("La tarjeta no existe.");

            if (!card.IsActive)
                throw new InvalidOperationException("La tarjeta no está activa.");

            var currentFee = await _feesService.GetCurrentFee();
            var feeAmount = amount * (currentFee?.FeePercentage ?? 0);

            if (card.Balance < amount + feeAmount)
                throw new InvalidOperationException("Saldo insuficiente.");


            card.Balance -= (amount + feeAmount);
            await _cardRepository.UpdateCard(card);

            return true;
        }

        public async Task<decimal> GetCardBalance(string cardNumber)
        {
            var card = await _cardRepository.GetByCardNumber(cardNumber);
            if (card == null)
                throw new InvalidOperationException("La tarjeta no existe.");


            return card.Balance;
        }
    }
}

