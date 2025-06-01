using BookingService.Dal.Interfaces;

namespace BookingService.Dal.Entities;

public class Room : IEntity
{
	public Guid Id { get; set; }
	public Guid HotelId { get; set; }
	public int Number { get; set; }
	public bool IsAvailable { get; set; }
	public decimal Price { get; set; }

	public Hotel Hotel { get; set; } = null!;
}
