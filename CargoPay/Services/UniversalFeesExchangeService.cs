using CargoPay.Entities;
using CargoPay.Infrastructure;
using CargoPay.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CargoPay.Services
{
    public class UniversalFeesExchangeService : IUniversalFeesExchangeService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private static readonly Random _random = new Random();
        private Timer _timer;

        public UniversalFeesExchangeService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _ = UpdateFee();
            var nextHour = DateTime.UtcNow.AddHours(1).Date.AddHours(DateTime.UtcNow.Hour + 1);
            var timeToNextHour = nextHour - DateTime.UtcNow;
            _timer = new Timer(UpdateFeeCallback, null, timeToNextHour, TimeSpan.FromHours(1));
        }

        public async Task<PaymentFee> GetCurrentFee()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                return await context.PaymentFees
                    .OrderByDescending(f => f.EffectiveDate)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task UpdateFee()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var currentHour = DateTime.UtcNow.Date.AddHours(DateTime.UtcNow.Hour);

                var existingFee = await context.PaymentFees
                    .FirstOrDefaultAsync(f => f.EffectiveDate == currentHour);

                if (existingFee == null)
                {
                    var currentFee = await GetCurrentFee();
                    var newFeePercentage = currentFee != null
                        ? currentFee.FeePercentage * (decimal)(_random.NextDouble() * 2)
                        : (decimal)(_random.NextDouble() * 2);

                    var newFee = new PaymentFee
                    {
                        FeePercentage = newFeePercentage,
                        EffectiveDate = currentHour
                    };

                    await context.PaymentFees.AddAsync(newFee);
                    await context.SaveChangesAsync();
                }
            }
        }

        private async void UpdateFeeCallback(object state)
        {
            await UpdateFee();
        }

        public async Task EnsureFeeIsUpToDate()
        {
            var currentFee = await GetCurrentFee();
            if (currentFee == null || (DateTime.UtcNow - currentFee.EffectiveDate).TotalHours >= 1)
            {
                await UpdateFee();
            }
        }
    }
}