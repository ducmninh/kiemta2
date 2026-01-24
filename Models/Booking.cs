using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM_396.Models
{
    [Table("396_Bookings")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; } = null!;
    }
}
