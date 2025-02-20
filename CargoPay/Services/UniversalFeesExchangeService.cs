using CargoPay.Entities;
using CargoPay.Infrastructure;
using CargoPay.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CargoPay.Services
{
    public class UniversalFeesExchangeService : IUniversalFeesExchangeService
    {
        private readonly ApplicationDbContext _context;
        private static readonly Random _random = new Random();

        public UniversalFeesExchangeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentFee> GetCurrentFee()
        {
            return await _context.PaymentFees.OrderByDescending(f => f.EffectiveDate).FirstOrDefaultAsync();
        }

        public async Task UpdateFee()
        {
            var currentFee = await GetCurrentFee();
            var newFeePercentage = currentFee != null
                ? currentFee.FeePercentage * (decimal)_random.NextDouble() * 2
                : (decimal)_random.NextDouble() * 2;

            var newFee = new PaymentFee
            {
                FeePercentage = newFeePercentage,
                EffectiveDate = DateTime.UtcNow
            };

            await _context.PaymentFees.AddAsync(newFee);
            await _context.SaveChangesAsync();
        }
    }
}