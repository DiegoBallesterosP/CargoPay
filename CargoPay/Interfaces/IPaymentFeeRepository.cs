using CargoPay.Entities;

namespace CargoPay.Interfaces
{
    public interface IPaymentFeeRepository
    {
        Task AddPaymentFee(PaymentFee paymentFee);
    }
}
