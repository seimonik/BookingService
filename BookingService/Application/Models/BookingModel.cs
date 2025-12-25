using BookingService.Dal.Enums;

namespace BookingService.Application.Models;

public class BookingModel
{
	public Guid Id { get; set; }
	public BookingStatus Status { get; set; }
	public decimal TotalPrice { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateOnly CheckInDate { get; set; }
	public DateOnly CheckOutDate { get; set; }
	public RoomTypeModel Room { get; set; } = null!;
	public ClientModel Client { get; set; } = null!;
}
