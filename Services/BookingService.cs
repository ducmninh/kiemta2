using Microsoft.EntityFrameworkCore;
using PCM_396.Data;
using PCM_396.Models;

namespace PCM_396.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(int memberId, DateTime startTime, DateTime endTime)
        {
            // Validation: Kiểm tra StartTime < EndTime
            if (startTime >= endTime)
            {
                throw new ArgumentException("Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc");
            }

            // Kiểm tra trùng lịch (Overlap)
            if (!await IsTimeSlotAvailableAsync(startTime, endTime))
            {
                throw new InvalidOperationException("Khoảng thời gian này đã có người đặt. Vui lòng chọn thời gian khác.");
            }

            var booking = new Booking
            {
                MemberId = memberId,
                StartTime = startTime,
                EndTime = endTime,
                CreatedDate = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> IsTimeSlotAvailableAsync(DateTime startTime, DateTime endTime, int? excludeBookingId = null)
        {
            // Logic kiểm tra overlap: A < D và B > C
            // Hai khoảng thời gian (A, B) và (C, D) trùng nhau khi: A < D và B > C
            var overlappingBookings = await _context.Bookings
                .Where(b => (excludeBookingId == null || b.Id != excludeBookingId) &&
                           b.StartTime < endTime && b.EndTime > startTime)
                .AnyAsync();

            return !overlappingBookings;
        }

        public async Task<List<Booking>> GetBookingsByMemberIdAsync(int memberId)
        {
            return await _context.Bookings
                .Include(b => b.Member)
                .Where(b => b.MemberId == memberId)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Member)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }
    }
}
