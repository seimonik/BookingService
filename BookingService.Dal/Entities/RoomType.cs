using BookingService.Dal.Interfaces;

namespace BookingService.Dal.Entities;

public class RoomType : IEntity
{
	public Guid Id { get; set; }
	public Guid HotelId { get; set; }
	public string Name { get; set; } = "";
	public decimal Price { get; set; }
	public int TotalCount { get; set; } // Общее кол-во физических номеров этого типа

	public Hotel Hotel { get; set; } = null!;
}
