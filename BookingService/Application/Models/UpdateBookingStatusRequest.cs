using BookingService.Dal.Enums;

namespace BookingService.Application.Models;

public class UpdateBookingStatusRequest
{
	public Guid BookingId { get; set; }
	public BookingStatus Status { get; set; }
}
