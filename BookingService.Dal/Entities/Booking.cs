using BookingService.Dal.Enums;
using BookingService.Dal.Interfaces;

namespace BookingService.Dal.Entities;

public class Booking : IEntity
{
	public Guid Id { get; set; }
	public Guid ClientId { get; set; }
	public Guid? RoomId { get; set; }
	public Guid? RoomTypeId { get; set; }
	public BookingStatus Status { get; set; }
	public decimal TotalPrice { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateOnly CheckInDate { get; set; }
	public DateOnly CheckOutDate { get; set; }
	public Guid IdempotencyKey { get; set; }
	public Guid? CardId { get; set; } // Для тестирования оплаты

	public Client Client { get; set; } = null!;
	public RoomType RoomType { get; set; } = null!;
	public Room Room { get; set; } = null!;
	public Card? Card { get; set; }
}
