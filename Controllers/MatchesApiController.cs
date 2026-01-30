using Microsoft.AspNetCore.Mvc;
using PCM_396.Services;
using PCM_396.Models;

namespace PCM_396.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesApiController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchesApiController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        /// <summary>
        /// Lấy danh sách tất cả trận đấu
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetAllMatches()
        {
            var matches = await _matchService.GetAllMatchesAsync();
            return Ok(matches);
        }

        /// <summary>
        /// Lấy lịch sử trận đấu của thành viên theo ID
        /// </summary>
        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatchesByMember(int memberId)
        {
            var matches = await _matchService.GetMatchesByMemberIdAsync(memberId);
            return Ok(matches);
        }

        /// <summary>
        /// Tạo trận đấu mới
        /// </summary>
        /// <param name="request">Thông tin trận đấu</param>
        [HttpPost]
        public async Task<ActionResult<Match>> CreateMatch([FromBody] CreateMatchRequest request)
        {
            try
            {
                var match = await _matchService.CreateMatchAsync(
                    request.Format,
                    request.IsRanked,
                    request.ChallengeId,
                    request.Winner1Id,
                    request.Winner2Id,
                    request.Loser1Id,
                    request.Loser2Id
                );
                return CreatedAtAction(nameof(GetAllMatches), new { id = match.Id }, match);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class CreateMatchRequest
    {
        public int Format { get; set; }
        public bool IsRanked { get; set; }
        public int? ChallengeId { get; set; }
        public int Winner1Id { get; set; }
        public int? Winner2Id { get; set; }
        public int Loser1Id { get; set; }
        public int? Loser2Id { get; set; }
    }
}
