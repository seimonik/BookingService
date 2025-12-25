namespace BookingService.Application.Models;

public class SendBookingNotificationRequest
{
	public Guid BookingId { get; set; }
	public decimal TotalPrice { get; set; }
	public DateOnly CheckInDate { get; set; }
	public DateOnly CheckOutDate { get; set; }
	public ClientModel Client { get; set; } = null!;
	public RoomNotificationRequest RoomType { get; set; } = null!;
}
