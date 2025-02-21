using CargoPay.Entities;
using CargoPay.Interfaces;
using CargoPay.Dtos;

namespace CargoPay.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUniversalFeesExchangeService _feesService;

        public CardService(ICardRepository cardRepository, ITransactionRepository transactionRepository, IUniversalFeesExchangeService feesService)
        {
            _cardRepository = cardRepository;
            _transactionRepository = transactionRepository;
            _feesService = feesService;
        }

        public async Task<Card> CreateCard(string cardNumber, decimal balance)
        {
            await _feesService.EnsureFeeIsUpToDate();

            if (await CardExists(cardNumber).ConfigureAwait(false))
            {
                throw new InvalidOperationException("Una tarjeta con este número ya existe.");
            }

            var card = new Card
            {
                CardNumber = cardNumber,
                Balance = balance,
                IsActive = true
            };

            await _cardRepository.AddCard(card).ConfigureAwait(false);
            return card;
        }

        public async Task<bool> CardExists(string cardNumber)
        {
            return await _cardRepository.CardExists(cardNumber).ConfigureAwait(false);
        }

        public async Task<PaymentResultDto> ProcessPayment(string cardNumber, decimal amount)
        {
            await _feesService.EnsureFeeIsUpToDate();

            var card = await _cardRepository.GetByCardNumber(cardNumber).ConfigureAwait(false);
            if (card == null)
            {
                return new PaymentResultDto { Success = false, Message = "La tarjeta no existe." };
            }

            var currentFee = await _feesService.GetCurrentFee().ConfigureAwait(false);
            var feeAmount = amount * (currentFee?.FeePercentage ?? 0);

            if (card.Balance < amount + feeAmount)
            {
                return new PaymentResultDto { Success = false, Message = "Saldo insuficiente." };
            }

            var previousBalance = card.Balance;

            card.Balance -= (amount + feeAmount);
            await _cardRepository.UpdateCard(card).ConfigureAwait(false);

            var transaction = new Transaction
            {
                CardId = card.Id,
                Amount = amount,
                FeeApplied = feeAmount,
                TransactionDate = DateTime.UtcNow,
                PreviousBalance = previousBalance,
                CardNumber = card.CardNumber
            };

            await _transactionRepository.AddTransaction(transaction).ConfigureAwait(false);

            return new PaymentResultDto { Success = true, Message = "Pago exitoso." };
        }

        public async Task<decimal> GetCardBalance(string cardNumber)
        {
            var card = await _cardRepository.GetByCardNumber(cardNumber).ConfigureAwait(false);
            if (card == null)
            {
                throw new InvalidOperationException("La tarjeta no existe.");
            }

            return card.Balance;
        }
    }
}