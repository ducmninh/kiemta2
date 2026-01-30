using Microsoft.AspNetCore.Mvc;
using PCM_396.Services;
using PCM_396.Models;

namespace PCM_396.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChallengesApiController : ControllerBase
    {
        private readonly IChallengeService _challengeService;

        public ChallengesApiController(IChallengeService challengeService)
        {
            _challengeService = challengeService;
        }

        /// <summary>
        /// Lấy danh sách tất cả thách đấu đang mở
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Challenge>>> GetAllChallenges()
        {
            var challenges = await _challengeService.GetOpenChallengesAsync();
            return Ok(challenges);
        }

        /// <summary>
        /// Lấy thông tin thách đấu theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Challenge>> GetChallenge(int id)
        {
            var challenge = await _challengeService.GetChallengeByIdAsync(id);
            if (challenge == null)
            {
                return NotFound(new { message = $"Không tìm thấy thách đấu với ID {id}" });
            }
            return Ok(challenge);
        }

        /// <summary>
        /// Lấy danh sách thách đấu đang mở
        /// </summary>
        [HttpGet("open")]
        public async Task<ActionResult<IEnumerable<Challenge>>> GetOpenChallenges()
        {
            var challenges = await _challengeService.GetOpenChallengesAsync();
            return Ok(challenges);
        }
    }
}
