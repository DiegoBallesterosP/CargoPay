using CargoPay.Entities;


namespace CargoPay.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddTransaction(Transaction transaction);
    }
}
