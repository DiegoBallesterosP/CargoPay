namespace CargoPay.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public virtual Card Card { get; set; }

        public decimal Amount { get; set; }

        public decimal FeeApplied { get; set; }

        public DateTime TransactionDate { get; set; }

    }
}
