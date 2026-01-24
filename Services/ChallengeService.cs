using Microsoft.EntityFrameworkCore;
using PCM_396.Data;
using PCM_396.Models;

namespace PCM_396.Services
{
    public class ChallengeService : IChallengeService
    {
        private readonly ApplicationDbContext _context;

        public ChallengeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Challenge>> GetOpenChallengesAsync()
        {
            return await _context.Challenges
                .Include(c => c.Creator)
                .Where(c => c.Status == (int)ChallengeStatus.Open)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Challenge>> GetAcceptedChallengesAsync()
        {
            return await _context.Challenges
                .Include(c => c.Creator)
                .Where(c => c.Status == (int)ChallengeStatus.Accepted)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<Challenge?> GetChallengeByIdAsync(int id)
        {
            return await _context.Challenges
                .Include(c => c.Creator)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Challenge> CreateChallengeAsync(int creatorId, string title, string? description)
        {
            var challenge = new Challenge
            {
                CreatorId = creatorId,
                Title = title,
                Description = description,
                Status = (int)ChallengeStatus.Open,
                CreatedDate = DateTime.Now
            };

            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync();
            return challenge;
        }

        public async Task AcceptChallengeAsync(int challengeId)
        {
            var challenge = await _context.Challenges.FindAsync(challengeId);
            if (challenge != null && challenge.Status == (int)ChallengeStatus.Open)
            {
                challenge.Status = (int)ChallengeStatus.Accepted;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CompleteChallengeAsync(int challengeId)
        {
            var challenge = await _context.Challenges.FindAsync(challengeId);
            if (challenge != null)
            {
                challenge.Status = (int)ChallengeStatus.Completed;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Challenge>> SearchChallengesAsync(string keyword)
        {
            return await _context.Challenges
                .Include(c => c.Creator)
                .Where(c => c.Status == (int)ChallengeStatus.Open &&
                           (c.Title.Contains(keyword) || 
                            (c.Description != null && c.Description.Contains(keyword))))
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }
    }
}
