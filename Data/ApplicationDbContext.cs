using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PCM_396.Models;

namespace PCM_396.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình relationships cho Match
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Winner1)
                .WithMany(mem => mem.MatchesAsWinner1)
                .HasForeignKey(m => m.Winner1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Winner2)
                .WithMany(mem => mem.MatchesAsWinner2)
                .HasForeignKey(m => m.Winner2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Loser1)
                .WithMany(mem => mem.MatchesAsLoser1)
                .HasForeignKey(m => m.Loser1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Loser2)
                .WithMany(mem => mem.MatchesAsLoser2)
                .HasForeignKey(m => m.Loser2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Challenge)
                .WithMany(c => c.Matches)
                .HasForeignKey(m => m.ChallengeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình cho Challenge
            modelBuilder.Entity<Challenge>()
                .HasOne(c => c.Creator)
                .WithMany(m => m.CreatedChallenges)
                .HasForeignKey(c => c.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình cho Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Member)
                .WithMany(m => m.Bookings)
                .HasForeignKey(b => b.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            // SEEDING DATA - QUAN TRỌNG cho giảng viên kiểm tra
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Members - ít nhất 5 members
            modelBuilder.Entity<Member>().HasData(
                new Member
                {
                    Id = 1,
                    IdentityUserId = "seed-user-1",
                    FullName = "Nguyễn Văn An (Admin)",
                    Status = "Active",
                    RankLevel = 5.0
                },
                new Member
                {
                    Id = 2,
                    IdentityUserId = "seed-user-2",
                    FullName = "Trần Thị Bình",
                    Status = "Active",
                    RankLevel = 3.5
                },
                new Member
                {
                    Id = 3,
                    IdentityUserId = "seed-user-3",
                    FullName = "Lê Văn Cường",
                    Status = "Active",
                    RankLevel = 4.2
                },
                new Member
                {
                    Id = 4,
                    IdentityUserId = "seed-user-4",
                    FullName = "Phạm Thị Dung",
                    Status = "Active",
                    RankLevel = 2.8
                },
                new Member
                {
                    Id = 5,
                    IdentityUserId = "seed-user-5",
                    FullName = "Hoàng Văn Em",
                    Status = "Active",
                    RankLevel = 3.9
                }
            );

            // Seed Challenges - ít nhất 3 challenges với các status khác nhau
            var baseDate = new DateTime(2026, 1, 24, 12, 0, 0);
            modelBuilder.Entity<Challenge>().HasData(
                new Challenge
                {
                    Id = 1,
                    CreatorId = 1,
                    Title = "Thách đấu buổi sáng thứ 7",
                    Description = "Tìm đối thủ xứng tầm cho trận đấu sáng thứ 7 tuần này",
                    Status = (int)ChallengeStatus.Open,
                    CreatedDate = baseDate.AddDays(-2)
                },
                new Challenge
                {
                    Id = 2,
                    CreatorId = 2,
                    Title = "Kèo đôi chiều chủ nhật",
                    Description = "Cần 1 cặp đôi chơi ranked match",
                    Status = (int)ChallengeStatus.Accepted,
                    CreatedDate = baseDate.AddDays(-1)
                },
                new Challenge
                {
                    Id = 3,
                    CreatorId = 3,
                    Title = "Giao hữu tối thứ 6",
                    Description = "Chơi vui vẻ không tính điểm",
                    Status = (int)ChallengeStatus.Completed,
                    CreatedDate = baseDate.AddDays(-5)
                }
            );

            // Seed Matches - ít nhất 2 matches (1 đơn, 1 đôi)
            modelBuilder.Entity<Match>().HasData(
                new Match
                {
                    Id = 1,
                    MatchDate = baseDate.AddDays(-3),
                    Format = (int)MatchFormat.Singles,
                    IsRanked = true,
                    ChallengeId = 3,
                    Winner1Id = 1,
                    Winner2Id = null,
                    Loser1Id = 3,
                    Loser2Id = null
                },
                new Match
                {
                    Id = 2,
                    MatchDate = baseDate.AddDays(-1),
                    Format = (int)MatchFormat.Doubles,
                    IsRanked = true,
                    ChallengeId = null,
                    Winner1Id = 2,
                    Winner2Id = 4,
                    Loser1Id = 3,
                    Loser2Id = 5
                }
            );

            // Seed Bookings - ít nhất 2 bookings
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = 1,
                    MemberId = 1,
                    StartTime = new DateTime(2026, 1, 25, 8, 0, 0),
                    EndTime = new DateTime(2026, 1, 25, 10, 0, 0),
                    CreatedDate = baseDate
                },
                new Booking
                {
                    Id = 2,
                    MemberId = 2,
                    StartTime = new DateTime(2026, 1, 26, 14, 0, 0),
                    EndTime = new DateTime(2026, 1, 26, 16, 0, 0),
                    CreatedDate = baseDate
                }
            );
        }
    }
}
