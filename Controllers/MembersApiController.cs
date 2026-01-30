using Microsoft.AspNetCore.Mvc;
using PCM_396.Services;
using PCM_396.Models;

namespace PCM_396.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersApiController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersApiController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        /// <summary>
        /// Lấy danh sách tất cả thành viên
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetAllMembers()
        {
            var members = await _memberService.GetAllMembersAsync();
            return Ok(members);
        }

        /// <summary>
        /// Lấy thông tin thành viên theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound(new { message = $"Không tìm thấy thành viên với ID {id}" });
            }
            return Ok(member);
        }
    }
}
