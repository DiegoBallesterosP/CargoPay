using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CargoPay.Entities
{
    public class PaymentFee
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal FeePercentage { get; set; }

        [Required]
        public DateTime EffectiveDate { get; set; }
    }
}