using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM_396.Models;
using PCM_396.Services;
using System.ComponentModel.DataAnnotations;

namespace PCM_396.Pages.Bookings
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IMemberService _memberService;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(IBookingService bookingService, IMemberService memberService, UserManager<IdentityUser> userManager)
        {
            _bookingService = bookingService;
            _memberService = memberService;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<Booking> RecentBookings { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Vui lòng chọn thời gian bắt đầu")]
            public DateTime StartTime { get; set; } = DateTime.Now.AddHours(1);

            [Required(ErrorMessage = "Vui lòng chọn thời gian kết thúc")]
            public DateTime EndTime { get; set; } = DateTime.Now.AddHours(2);
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var member = await _memberService.GetMemberByIdentityUserIdAsync(user.Id);
                if (member != null)
                {
                    RecentBookings = await _bookingService.GetBookingsByMemberIdAsync(member.Id);
                    RecentBookings = RecentBookings.Take(5).ToList();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Validation: StartTime < EndTime
            if (Input.StartTime >= Input.EndTime)
            {
                ModelState.AddModelError(string.Empty, "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc");
                await OnGetAsync();
                return Page();
            }

            // Validation: Không được đặt trong quá khứ
            if (Input.StartTime < DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Không thể đặt sân trong quá khứ");
                await OnGetAsync();
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

                await _bookingService.CreateBookingAsync(member.Id, Input.StartTime, Input.EndTime);
                TempData["Message"] = "Đặt sân thành công! Hẹn gặp bạn tại sân.";
                return RedirectToPage();
            }
            catch (InvalidOperationException ex)
            {
                // Lỗi trùng lịch
                ModelState.AddModelError(string.Empty, ex.Message);
                await OnGetAsync();
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra: {ex.Message}");
                await OnGetAsync();
                return Page();
            }
        }
    }
}
