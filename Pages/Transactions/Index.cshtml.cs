using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM_396.Models;
using PCM_396.Services;

namespace PCM_396.Pages.Transactions
{
    [Authorize(Roles = "Admin,Moderator")]
    public class IndexModel : PageModel
    {
        private readonly ITransactionService _transactionService;
        private readonly IMemberService _memberService;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(
            ITransactionService transactionService,
            IMemberService memberService,
            UserManager<IdentityUser> userManager)
        {
            _transactionService = transactionService;
            _memberService = memberService;
            _userManager = userManager;
        }

        public List<Transaction> Transactions { get; set; } = new();
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? FilterType { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? FilterCategory { get; set; }

        public List<string> Categories { get; set; } = new()
        {
            "Quỹ tháng",
            "Nước",
            "Điện",
            "Bảo trì",
            "Thiết bị",
            "Khác"
        };

        public async Task OnGetAsync()
        {
            Transactions = await _transactionService.GetTransactionsByFilterAsync(FilterType, FilterCategory);
            TotalIncome = await _transactionService.GetTotalIncomeAsync();
            TotalExpense = await _transactionService.GetTotalExpenseAsync();
            Balance = await _transactionService.GetBalanceAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var success = await _transactionService.DeleteTransactionAsync(id);
            if (success)
            {
                TempData["Message"] = "Xóa giao dịch thành công!";
            }
            else
            {
                TempData["Error"] = "Không thể xóa giao dịch!";
            }
            return RedirectToPage();
        }
    }
}
