using PCM_396.Models;

namespace PCM_396.Services
{
    public interface IMatchService
    {
        Task<Match> CreateMatchAsync(int format, bool isRanked, int? challengeId, 
            int winner1Id, int? winner2Id, int loser1Id, int? loser2Id);
        Task<List<Match>> GetAllMatchesAsync();
        Task<List<Match>> GetMatchesByMemberIdAsync(int memberId);
    }
}
