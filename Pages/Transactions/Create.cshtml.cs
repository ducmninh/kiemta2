using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM_396.Models;
using PCM_396.Services;

namespace PCM_396.Pages.Transactions
{
    [Authorize(Roles = "Admin,Moderator")]
    public class CreateModel : PageModel
    {
        private readonly ITransactionService _transactionService;
        private readonly IMemberService _memberService;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(
            ITransactionService transactionService,
            IMemberService memberService,
            UserManager<IdentityUser> userManager)
        {
            _transactionService = transactionService;
            _memberService = memberService;
            _userManager = userManager;
        }

        [BindProperty]
        public Transaction Transaction { get; set; } = new();

        public List<string> Categories { get; set; } = new()
        {
            "Quỹ tháng",
            "Nước",
            "Điện",
            "Bảo trì",
            "Thiết bị",
            "Khác"
        };

        public void OnGet()
        {
            Transaction.TransactionDate = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var member = await _memberService.GetMemberByIdentityUserIdAsync(user.Id);
            if (member == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin thành viên!";
                return Page();
            }

            Transaction.CreatorId = member.Id;
            var success = await _transactionService.CreateTransactionAsync(Transaction);

            if (success)
            {
                TempData["Message"] = "Thêm giao dịch thành công!";
                return RedirectToPage("Index");
            }

            TempData["Error"] = "Không thể thêm giao dịch!";
            return Page();
        }
    }
}
