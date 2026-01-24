using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM_396.Models;
using PCM_396.Services;

namespace PCM_396.Pages.Challenges
{
    [Authorize(Roles = "Admin")]
    public class ManageModel : PageModel
    {
        private readonly IChallengeService _challengeService;

        public ManageModel(IChallengeService challengeService)
        {
            _challengeService = challengeService;
        }

        public List<Challenge> Challenges { get; set; } = new();
        
        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; } = "open";

        public async Task OnGetAsync()
        {
            var allChallenges = await _challengeService.GetOpenChallengesAsync();
            
            switch (Filter.ToLower())
            {
                case "accepted":
                    allChallenges = await _challengeService.GetAcceptedChallengesAsync();
                    break;
                case "completed":
                    // Lấy tất cả và filter completed
                    allChallenges = (await _challengeService.GetOpenChallengesAsync())
                        .Where(c => c.Status == (int)ChallengeStatus.Completed).ToList();
                    break;
                default:
                    allChallenges = await _challengeService.GetOpenChallengesAsync();
                    break;
            }
            
            Challenges = allChallenges;
        }
    }
}
