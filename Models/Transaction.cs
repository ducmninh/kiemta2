using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM_396.Models
{
    [Table("396_Transactions")]
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // "Thu" or "Chi"

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public virtual Member Creator { get; set; } = null!;

        public DateTime CreatedDate { get; set; }
    }
}
