namespace BookingService.Application.Models;

public class AddBookingModel
{
	//public Guid RoomId { get; set; }
	public Guid RoomTypeId { get; set; }
	//public Guid ClientId { get; set; }
	public DateOnly CheckInDate { get; set; }
	public DateOnly CheckOutDate { get; set; }
	public Guid IdempotencyKey { get; set; }
	public string? Promocode { get; set; }
	public string Email { get; set; } = "";
	public string FullName { get; set; } = "";
}
