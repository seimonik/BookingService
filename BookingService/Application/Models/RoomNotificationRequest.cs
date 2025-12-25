namespace BookingService.Application.Models;

public class RoomNotificationRequest
{
	//public int Number { get; set; }
	public string TypeName { get; set; } = "";
	public HotelNotificationRequest Hotel { get; set; } = null!;
}
