namespace CargoPay.Entities
{
    public class PaymentFee
    {
        public int Id { get; set; }

        public decimal FeePercentage { get; set; }

        public DateTime EffectiveDate { get; set; }
    }
}
