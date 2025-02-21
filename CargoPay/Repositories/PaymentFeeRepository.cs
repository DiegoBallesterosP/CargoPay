using CargoPay.Entities;
using CargoPay.Infrastructure;
using CargoPay.Interfaces;

namespace CargoPay.Repositories
{
    public class PaymentFeeRepository : IPaymentFeeRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentFeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddPaymentFee(PaymentFee paymentFee)
        {
            await _context.PaymentFees.AddAsync(paymentFee);
            await _context.SaveChangesAsync();
        }
    }
}