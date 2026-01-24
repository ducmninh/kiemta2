using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM_396.Models
{
    [Table("396_Members")]
    public class Member
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string IdentityUserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active hoặc Block

        [Required]
        public double RankLevel { get; set; } = 1.0;

        // Navigation properties
        public virtual ICollection<Challenge> CreatedChallenges { get; set; } = new List<Challenge>();
        public virtual ICollection<Match> MatchesAsWinner1 { get; set; } = new List<Match>();
        public virtual ICollection<Match> MatchesAsWinner2 { get; set; } = new List<Match>();
        public virtual ICollection<Match> MatchesAsLoser1 { get; set; } = new List<Match>();
        public virtual ICollection<Match> MatchesAsLoser2 { get; set; } = new List<Match>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
