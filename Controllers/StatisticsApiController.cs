using Microsoft.AspNetCore.Mvc;
using PCM_396.Services;

namespace PCM_396.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IMatchService _matchService;
        private readonly IBookingService _bookingService;
        private readonly ITransactionService _transactionService;
        private readonly IChallengeService _challengeService;

        public StatisticsApiController(
            IMemberService memberService,
            IMatchService matchService,
            IBookingService bookingService,
            ITransactionService transactionService,
            IChallengeService challengeService)
        {
            _memberService = memberService;
            _matchService = matchService;
            _bookingService = bookingService;
            _transactionService = transactionService;
            _challengeService = challengeService;
        }

        /// <summary>
        /// Lấy tổng quan thống kê hệ thống
        /// </summary>
        [HttpGet("overview")]
        public async Task<ActionResult> GetOverview()
        {
            var members = await _memberService.GetAllMembersAsync();
            var matches = await _matchService.GetAllMatchesAsync();
            var bookings = await _bookingService.GetAllBookingsAsync();
            var openChallenges = await _challengeService.GetOpenChallengesAsync();
            var totalIncome = await _transactionService.GetTotalIncomeAsync();
            var totalExpense = await _transactionService.GetTotalExpenseAsync();
            var balance = await _transactionService.GetBalanceAsync();

            return Ok(new
            {
                totalMembers = members.Count,
                totalMatches = matches.Count,
                totalBookings = bookings.Count,
                openChallenges = openChallenges.Count,
                financial = new
                {
                    totalIncome = totalIncome,
                    totalExpense = totalExpense,
                    balance = balance
                }
            });
        }

        /// <summary>
        /// Thống kê theo thành viên
        /// </summary>
        [HttpGet("member/{memberId}")]
        public async Task<ActionResult> GetMemberStatistics(int memberId)
        {
            var member = await _memberService.GetMemberByIdAsync(memberId);
            if (member == null)
            {
                return NotFound(new { message = $"Không tìm thấy thành viên với ID {memberId}" });
            }

            var matches = await _matchService.GetMatchesByMemberIdAsync(memberId);
            var bookings = await _bookingService.GetBookingsByMemberIdAsync(memberId);

            return Ok(new
            {
                member = new
                {
                    id = member.Id,
                    fullName = member.FullName,
                    rankLevel = member.RankLevel,
                    status = member.Status
                },
                statistics = new
                {
                    totalMatches = matches.Count,
                    totalBookings = bookings.Count
                }
            });
        }
    }
}
