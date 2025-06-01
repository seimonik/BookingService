using BookingService.Dal.Enums;
using BookingService.Dal.Interfaces;

namespace BookingService.Dal.Entities;

public class Booking : IEntity
{
	public Guid Id { get; set; }
	public Guid ClientId { get; set; }
	public Guid RoomId { get; set; }
	public BookingStatus Status { get; set; }
	public decimal TotalPrice { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateOnly CheckInDate { get; set; }
	public DateOnly CheckOutDate { get; set; }

	public Client Client { get; set; } = null!;
	public Room Room { get; set; } = null!;
}
