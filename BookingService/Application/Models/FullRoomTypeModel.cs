namespace BookingService.Application.Models;

public class FullRoomTypeModel
{
	public Guid Id { get; set; }
	public string Name { get; set; } = "";
	public decimal Price { get; set; }
	public HotelMiniModel Hotel { get; set; } = null!;
	public CancellationPolicyModel? CancellationPolicy { get; set; }
}