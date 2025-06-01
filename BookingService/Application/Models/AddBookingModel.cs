namespace BookingService.Application.Models;

public class AddBookingModel
{
	public Guid RoomId { get; set; }
	public Guid ClientId { get; set; }	
	public DateOnly CheckInDate { get; set; }
	public DateOnly CheckOutDate { get; set; }
}
