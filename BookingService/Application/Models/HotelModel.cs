namespace BookingService.Application.Models;

public class HotelModel
{
	public Guid Id { get; set; }
	public string Name { get; set; } = "";
	public string Address { get; set; } = "";
	public string Country { get; set; } = "";
	public IEnumerable<RoomModel> Rooms { get; set; } = [];
}
