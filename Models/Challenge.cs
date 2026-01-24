using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM_396.Models
{
    [Table("396_Challenges")]
    public class Challenge
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int Status { get; set; } = 0; // 0-Open, 1-Accepted, 2-Completed

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("CreatorId")]
        public virtual Member Creator { get; set; } = null!;

        public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
    }

    // Enum cho Status
    public enum ChallengeStatus
    {
        Open = 0,
        Accepted = 1,
        Completed = 2
    }
}
