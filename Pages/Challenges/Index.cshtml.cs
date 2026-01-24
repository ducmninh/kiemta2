using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM_396.Models;
using PCM_396.Services;

namespace PCM_396.Pages.Challenges
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IChallengeService _challengeService;
        private readonly IMemberService _memberService;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(IChallengeService challengeService, IMemberService memberService, UserManager<IdentityUser> userManager)
        {
            _challengeService = challengeService;
            _memberService = memberService;
            _userManager = userManager;
        }

        public List<Challenge> Challenges { get; set; } = new();
        public Member? CurrentMember { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? SearchKeyword { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                CurrentMember = await _memberService.GetMemberByIdentityUserIdAsync(user.Id);
                
                // Tạo Member nếu chưa có
                if (CurrentMember == null)
                {
                    CurrentMember = await _memberService.CreateMemberAsync(user.Id, user.UserName ?? "User");
                    
                    // Gán role Member nếu chưa có
                    if (!await _userManager.IsInRoleAsync(user, "Member"))
                    {
                        await _userManager.AddToRoleAsync(user, "Member");
                    }
                }
            }

            // Tìm kiếm hoặc lấy tất cả kèo Open
            if (!string.IsNullOrEmpty(SearchKeyword))
            {
                Challenges = await _challengeService.SearchChallengesAsync(SearchKeyword);
            }
            else
            {
                Challenges = await _challengeService.GetOpenChallengesAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAcceptAsync(int id)
        {
            try
            {
                await _challengeService.AcceptChallengeAsync(id);
                TempData["Message"] = "Bạn đã nhận kèo thành công! Hẹn gặp trên sân.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Có lỗi xảy ra: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
