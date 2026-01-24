using PCM_396.Models;

namespace PCM_396.Services
{
    public interface IMemberService
    {
        Task<Member?> GetMemberByIdentityUserIdAsync(string identityUserId);
        Task<Member?> GetMemberByIdAsync(int id);
        Task<List<Member>> GetAllMembersAsync();
        Task<Member> CreateMemberAsync(string identityUserId, string fullName);
        Task UpdateMemberAsync(Member member);
        Task<bool> IsMemberExistsAsync(string identityUserId);
    }
}
