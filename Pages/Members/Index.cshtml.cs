using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PCM_396.Data;
using PCM_396.Models;

namespace PCM_396.Pages.Members
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Member> Members { get; set; } = new();

        public async Task OnGetAsync()
        {
            Members = await _context.Members
                .OrderByDescending(m => m.RankLevel)
                .ToListAsync();
        }
    }
}
