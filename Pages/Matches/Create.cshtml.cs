using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM_396.Models;
using PCM_396.Services;
using System.ComponentModel.DataAnnotations;

namespace PCM_396.Pages.Matches
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IMatchService _matchService;
        private readonly IChallengeService _challengeService;
        private readonly IMemberService _memberService;

        public CreateModel(IMatchService matchService, IChallengeService challengeService, IMemberService memberService)
        {
            _matchService = matchService;
            _challengeService = challengeService;
            _memberService = memberService;
        }

        public List<Challenge> AcceptedChallenges { get; set; } = new();
        public List<Member> Members { get; set; } = new();

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Vui lòng chọn loại trận đấu")]
            public int Format { get; set; } // 0: Singles, 1: Doubles

            public int? ChallengeId { get; set; }

            [Required(ErrorMessage = "Vui lòng chọn Winner 1")]
            public int Winner1Id { get; set; }

            public int? Winner2Id { get; set; }

            [Required(ErrorMessage = "Vui lòng chọn Loser 1")]
            public int Loser1Id { get; set; }

            public int? Loser2Id { get; set; }

            public bool IsRanked { get; set; } = true;
        }

        public async Task OnGetAsync()
        {
            AcceptedChallenges = await _challengeService.GetAcceptedChallengesAsync();
            Members = await _memberService.GetAllMembersAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Load lại data nếu có lỗi
            AcceptedChallenges = await _challengeService.GetAcceptedChallengesAsync();
            Members = await _memberService.GetAllMembersAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Validation: Nếu Doubles thì phải có đủ Winner2 và Loser2
            if (Input.Format == (int)MatchFormat.Doubles)
            {
                if (!Input.Winner2Id.HasValue || !Input.Loser2Id.HasValue)
                {
                    ModelState.AddModelError(string.Empty, "Trận Đôi phải có đủ 4 người chơi (Winner1, Winner2, Loser1, Loser2)");
                    return Page();
                }
            }

            // Validation: Không được trùng người chơi
            var players = new List<int> { Input.Winner1Id, Input.Loser1Id };
            if (Input.Winner2Id.HasValue) players.Add(Input.Winner2Id.Value);
            if (Input.Loser2Id.HasValue) players.Add(Input.Loser2Id.Value);

            if (players.Distinct().Count() != players.Count)
            {
                ModelState.AddModelError(string.Empty, "Không được chọn trùng người chơi");
                return Page();
            }

            try
            {
                await _matchService.CreateMatchAsync(
                    Input.Format,
                    Input.IsRanked,
                    Input.ChallengeId,
                    Input.Winner1Id,
                    Input.Winner2Id,
                    Input.Loser1Id,
                    Input.Loser2Id
                );

                TempData["Message"] = "Kết quả trận đấu đã được lưu thành công!";
                return RedirectToPage("History");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra: {ex.Message}");
                return Page();
            }
        }
    }
}
