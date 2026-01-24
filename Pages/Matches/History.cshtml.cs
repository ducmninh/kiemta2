using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM_396.Models;
using PCM_396.Services;

namespace PCM_396.Pages.Matches
{
    [Authorize]
    public class HistoryModel : PageModel
    {
        private readonly IMatchService _matchService;

        public HistoryModel(IMatchService matchService)
        {
            _matchService = matchService;
        }

        public List<Match> Matches { get; set; } = new();

        public async Task OnGetAsync()
        {
            Matches = await _matchService.GetAllMatchesAsync();
        }
    }
}
