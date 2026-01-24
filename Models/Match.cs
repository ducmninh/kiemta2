using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM_396.Models
{
    [Table("396_Matches")]
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime MatchDate { get; set; } = DateTime.Now;

        [Required]
        public int Format { get; set; } // 0: Singles-Đơn, 1: Doubles-Đôi

        [Required]
        public bool IsRanked { get; set; } = true;

        // Link tới Challenge (nullable - có thể là trận tự do)
        public int? ChallengeId { get; set; }

        // Winners
        [Required]
        public int Winner1Id { get; set; }

        public int? Winner2Id { get; set; } // Nullable - chỉ có khi đánh Đôi

        // Losers
        [Required]
        public int Loser1Id { get; set; }

        public int? Loser2Id { get; set; } // Nullable - chỉ có khi đánh Đôi

        // Navigation properties
        [ForeignKey("ChallengeId")]
        public virtual Challenge? Challenge { get; set; }

        [ForeignKey("Winner1Id")]
        public virtual Member Winner1 { get; set; } = null!;

        [ForeignKey("Winner2Id")]
        public virtual Member? Winner2 { get; set; }

        [ForeignKey("Loser1Id")]
        public virtual Member Loser1 { get; set; } = null!;

        [ForeignKey("Loser2Id")]
        public virtual Member? Loser2 { get; set; }
    }

    // Enum cho Format
    public enum MatchFormat
    {
        Singles = 0,
        Doubles = 1
    }
}
