using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CargoPay.Entities
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public int CardId { get; set; }

        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal FeeApplied { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal PreviousBalance { get; set; }

        [StringLength(15)]
        [Required]
        public string CardNumber { get; set; }
    }
}