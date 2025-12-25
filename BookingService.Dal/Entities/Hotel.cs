using BookingService.Dal.Interfaces;

namespace BookingService.Dal.Entities;

public class Hotel : IEntity
{
	public Guid Id { get; set; }
	public string Name { get; set; } = "";
	public string Address { get; set; } = "";
	public string Country { get; set; } = "";

	public ICollection<Room> Rooms { get; set; } = null!;
	public ICollection<RoomType> RoomTypes { get; set; } = null!;
}
