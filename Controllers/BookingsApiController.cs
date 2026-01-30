using Microsoft.AspNetCore.Mvc;
using PCM_396.Services;
using PCM_396.Models;

namespace PCM_396.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsApiController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsApiController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Lấy danh sách tất cả đặt sân
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        /// <summary>
        /// Lấy danh sách đặt sân theo thành viên
        /// </summary>
        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByMember(int memberId)
        {
            var bookings = await _bookingService.GetBookingsByMemberIdAsync(memberId);
            return Ok(bookings);
        }

        /// <summary>
        /// Kiểm tra khung giờ còn trống không
        /// </summary>
        [HttpGet("check-availability")]
        public async Task<ActionResult<bool>> CheckAvailability(
            [FromQuery] DateTime startTime, 
            [FromQuery] DateTime endTime)
        {
            var isAvailable = await _bookingService.IsTimeSlotAvailableAsync(startTime, endTime);
            return Ok(new { available = isAvailable });
        }

        /// <summary>
        /// Tạo đặt sân mới
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking([FromBody] CreateBookingRequest request)
        {
            try
            {
                var booking = await _bookingService.CreateBookingAsync(
                    request.MemberId,
                    request.StartTime,
                    request.EndTime
                );
                return CreatedAtAction(nameof(GetAllBookings), new { id = booking.Id }, booking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class CreateBookingRequest
    {
        public int MemberId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
