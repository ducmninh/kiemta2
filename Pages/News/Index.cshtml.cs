using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PCM_396.Data;
using PCM_396.Models;

namespace PCM_396.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Match> TodayMatches { get; set; } = new();
        public List<Challenge> OpenChallenges { get; set; } = new();
        public List<Challenge> AcceptedChallenges { get; set; } = new();

        public async Task OnGetAsync()
        {
            var today = DateTime.Today;

            // Get today's matches
            TodayMatches = await _context.Matches
                .Include(m => m.Winner1)
                .Include(m => m.Winner2)
                .Include(m => m.Loser1)
                .Include(m => m.Loser2)
                .Include(m => m.Challenge)
                .Where(m => m.MatchDate.Date == today)
                .OrderByDescending(m => m.MatchDate)
                .ToListAsync();

            // Get open challenges (Status = 0)
            OpenChallenges = await _context.Challenges
                .Include(c => c.Creator)
                .Where(c => c.Status == 0)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            // Get accepted challenges (Status = 1)
            AcceptedChallenges = await _context.Challenges
                .Include(c => c.Creator)
                .Where(c => c.Status == 1)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }
    }
}
