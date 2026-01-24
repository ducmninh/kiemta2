using PCM_396.Models;

namespace PCM_396.Services
{
    public interface IChallengeService
    {
        Task<List<Challenge>> GetOpenChallengesAsync();
        Task<List<Challenge>> GetAcceptedChallengesAsync();
        Task<Challenge?> GetChallengeByIdAsync(int id);
        Task<Challenge> CreateChallengeAsync(int creatorId, string title, string? description);
        Task AcceptChallengeAsync(int challengeId);
        Task CompleteChallengeAsync(int challengeId);
        Task<List<Challenge>> SearchChallengesAsync(string keyword);
    }
}
