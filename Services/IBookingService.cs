using PCM_396.Models;

namespace PCM_396.Services
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(int memberId, DateTime startTime, DateTime endTime);
        Task<bool> IsTimeSlotAvailableAsync(DateTime startTime, DateTime endTime, int? excludeBookingId = null);
        Task<List<Booking>> GetBookingsByMemberIdAsync(int memberId);
        Task<List<Booking>> GetAllBookingsAsync();
    }
}
