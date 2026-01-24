using Microsoft.EntityFrameworkCore;
using PCM_396.Data;
using PCM_396.Models;

namespace PCM_396.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;
        private readonly IChallengeService _challengeService;

        public MatchService(ApplicationDbContext context, IChallengeService challengeService)
        {
            _context = context;
            _challengeService = challengeService;
        }

        public async Task<Match> CreateMatchAsync(int format, bool isRanked, int? challengeId,
            int winner1Id, int? winner2Id, int loser1Id, int? loser2Id)
        {
            // Validation
            if (format == (int)MatchFormat.Doubles)
            {
                if (winner2Id == null || loser2Id == null)
                {
                    throw new ArgumentException("Trận Đôi phải có đủ 4 người chơi");
                }
            }

            // Tạo Match
            var match = new Match
            {
                MatchDate = DateTime.Now,
                Format = format,
                IsRanked = isRanked,
                ChallengeId = challengeId,
                Winner1Id = winner1Id,
                Winner2Id = winner2Id,
                Loser1Id = loser1Id,
                Loser2Id = loser2Id
            };

            _context.Matches.Add(match);

            // Nếu match thuộc về Challenge, cập nhật status Challenge thành Completed
            if (challengeId.HasValue)
            {
                await _challengeService.CompleteChallengeAsync(challengeId.Value);
            }

            // Nếu IsRanked = true, cập nhật điểm Rank
            if (isRanked)
            {
                await UpdateRankPointsAsync(winner1Id, winner2Id, loser1Id, loser2Id);
            }

            await _context.SaveChangesAsync();
            return match;
        }

        private async Task UpdateRankPointsAsync(int winner1Id, int? winner2Id, int loser1Id, int? loser2Id)
        {
            // Cộng điểm cho Winners
            var winner1 = await _context.Members.FindAsync(winner1Id);
            if (winner1 != null)
            {
                winner1.RankLevel += 0.1;
            }

            if (winner2Id.HasValue)
            {
                var winner2 = await _context.Members.FindAsync(winner2Id.Value);
                if (winner2 != null)
                {
                    winner2.RankLevel += 0.1;
                }
            }

            // Trừ điểm cho Losers (tối thiểu 1.0)
            var loser1 = await _context.Members.FindAsync(loser1Id);
            if (loser1 != null)
            {
                loser1.RankLevel = Math.Max(1.0, loser1.RankLevel - 0.1);
            }

            if (loser2Id.HasValue)
            {
                var loser2 = await _context.Members.FindAsync(loser2Id.Value);
                if (loser2 != null)
                {
                    loser2.RankLevel = Math.Max(1.0, loser2.RankLevel - 0.1);
                }
            }
        }

        public async Task<List<Match>> GetAllMatchesAsync()
        {
            return await _context.Matches
                .Include(m => m.Winner1)
                .Include(m => m.Winner2)
                .Include(m => m.Loser1)
                .Include(m => m.Loser2)
                .Include(m => m.Challenge)
                .OrderByDescending(m => m.MatchDate)
                .ToListAsync();
        }

        public async Task<List<Match>> GetMatchesByMemberIdAsync(int memberId)
        {
            return await _context.Matches
                .Include(m => m.Winner1)
                .Include(m => m.Winner2)
                .Include(m => m.Loser1)
                .Include(m => m.Loser2)
                .Include(m => m.Challenge)
                .Where(m => m.Winner1Id == memberId || m.Winner2Id == memberId ||
                           m.Loser1Id == memberId || m.Loser2Id == memberId)
                .OrderByDescending(m => m.MatchDate)
                .ToListAsync();
        }
    }
}
