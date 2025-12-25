namespace BookingService.Application.Models;

public class RoomTypeModel
{
	public Guid Id { get; set; }
	public string Name { get; set; } = "";
	public decimal Price { get; set; }
	public int TotalCount { get; set; }
}
