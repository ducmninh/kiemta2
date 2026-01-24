using Microsoft.EntityFrameworkCore;
using PCM_396.Data;
using PCM_396.Models;

namespace PCM_396.Services
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _context;

        public MemberService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Member?> GetMemberByIdentityUserIdAsync(string identityUserId)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.IdentityUserId == identityUserId);
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _context.Members.FindAsync(id);
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _context.Members
                .Where(m => m.Status == "Active")
                .OrderByDescending(m => m.RankLevel)
                .ToListAsync();
        }

        public async Task<Member> CreateMemberAsync(string identityUserId, string fullName)
        {
            var member = new Member
            {
                IdentityUserId = identityUserId,
                FullName = fullName,
                Status = "Active",
                RankLevel = 1.0
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task UpdateMemberAsync(Member member)
        {
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsMemberExistsAsync(string identityUserId)
        {
            return await _context.Members
                .AnyAsync(m => m.IdentityUserId == identityUserId);
        }
    }
}
