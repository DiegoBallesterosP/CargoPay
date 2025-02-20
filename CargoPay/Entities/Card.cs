using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoPay.Entities
{
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [StringLength(15)]
        [Required]
        public string CardNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Balance { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}