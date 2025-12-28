using BookingService.Dal.Enums;

namespace BookingService.Application.Models;

public class FullBookingModel
{
	public Guid Id { get; set; }
	public BookingStatus Status { get; set; }
	public decimal TotalPrice { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateOnly CheckInDate { get; set; }
	public DateOnly CheckOutDate { get; set; }
	public FullRoomTypeModel RoomType { get; set; } = null!;
}
