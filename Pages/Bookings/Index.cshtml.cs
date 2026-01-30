using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM_396.Models;
using PCM_396.Services;

namespace PCM_396.Pages.Bookings
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IMemberService _memberService;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(IBookingService bookingService, IMemberService memberService, UserManager<IdentityUser> userManager)
        {
            _bookingService = bookingService;
            _memberService = memberService;
            _userManager = userManager;
        }

        public List<TimeSlot> TimeSlots { get; set; } = new();
        public List<Booking> MyUpcomingBookings { get; set; } = new();
        public int TotalAvailableSlots { get; set; }
        public int MyTotalBookings { get; set; }

        public async Task OnGetAsync()
        {
            // Get current user
            var user = await _userManager.GetUserAsync(User);
            Member? currentMember = null;

            if (user != null)
            {
                currentMember = await _memberService.GetMemberByIdentityUserIdAsync(user.Id);
            }

            // Get all bookings for today
            var allBookings = await _bookingService.GetAllBookingsAsync();
            var today = DateTime.Today;
            var todayBookings = allBookings.Where(b => b.StartTime.Date == today).ToList();

            // Generate time slots from 6:00 to 21:00 (every hour)
            TimeSlots = new List<TimeSlot>();
            for (int hour = 6; hour <= 21; hour++)
            {
                var startTime = new DateTime(today.Year, today.Month, today.Day, hour, 0, 0);
                var endTime = startTime.AddHours(1);

                // Check if this slot is booked
                var booking = todayBookings.FirstOrDefault(b => 
                    b.StartTime <= startTime && b.EndTime >= endTime);

                var slot = new TimeSlot
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    IsPast = startTime < DateTime.Now,
                    IsBooked = booking != null,
                    IsMine = booking != null && currentMember != null && booking.MemberId == currentMember.Id
                };

                TimeSlots.Add(slot);
            }

            // Calculate statistics
            TotalAvailableSlots = TimeSlots.Count(s => !s.IsBooked && !s.IsPast);
            
            // Get user's upcoming bookings
            if (currentMember != null)
            {
                var myBookings = await _bookingService.GetBookingsByMemberIdAsync(currentMember.Id);
                MyUpcomingBookings = myBookings
                    .Where(b => b.StartTime >= DateTime.Now)
                    .OrderBy(b => b.StartTime)
                    .ToList();
                MyTotalBookings = MyUpcomingBookings.Count;
            }
        }

        public class TimeSlot
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public bool IsPast { get; set; }
            public bool IsBooked { get; set; }
            public bool IsMine { get; set; }
        }
    }
}
