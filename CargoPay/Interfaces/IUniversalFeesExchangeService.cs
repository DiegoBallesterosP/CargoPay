using CargoPay.Entities;

namespace CargoPay.Interfaces
{
    public interface IUniversalFeesExchangeService
    {
        Task<PaymentFee> GetCurrentFee();
        Task UpdateFee();
    }
}