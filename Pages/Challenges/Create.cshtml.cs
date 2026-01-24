using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM_396.Services;
using System.ComponentModel.DataAnnotations;

namespace PCM_396.Pages.Challenges
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IChallengeService _challengeService;
        private readonly IMemberService _memberService;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(IChallengeService challengeService, IMemberService memberService, UserManager<IdentityUser> userManager)
        {
            _challengeService = challengeService;
            _memberService = memberService;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Vui lòng nhập tiêu đề kèo")]
            [StringLength(200, ErrorMessage = "Tiêu đề không được dài quá 200 ký tự")]
            public string Title { get; set; } = string.Empty;

            [StringLength(1000, ErrorMessage = "Mô tả không được dài quá 1000 ký tự")]
            public string? Description { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                var member = await _memberService.GetMemberByIdentityUserIdAsync(user.Id);
                if (member == null)
                {
                    member = await _memberService.CreateMemberAsync(user.Id, user.UserName ?? "User");
                    
                    if (!await _userManager.IsInRoleAsync(user, "Member"))
                    {
                        await _userManager.AddToRoleAsync(user, "Member");
                    }
                }

                await _challengeService.CreateChallengeAsync(member.Id, Input.Title, Input.Description);
                TempData["Message"] = "Kèo đã được tạo thành công! Chờ đối thủ nhận kèo.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra: {ex.Message}");
                return Page();
            }
        }
    }
}
