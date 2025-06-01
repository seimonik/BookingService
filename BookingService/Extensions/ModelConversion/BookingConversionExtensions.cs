using BookingService.Application.Models;
using BookingService.Dal.Entities;

namespace BookingService.Extensions.ModelConversion;

public static class BookingConversionExtensions
{
	public static Booking ToBooking(this AddBookingModel bookingModel, decimal totalPrice) =>
		new()
		{
			Id = Guid.NewGuid(),
			ClientId = bookingModel.ClientId,
			RoomId = bookingModel.RoomId,
			Status = Dal.Enums.BookingStatus.Pending,
			TotalPrice = totalPrice,
			CheckInDate = bookingModel.CheckInDate,
			CheckOutDate = bookingModel.CheckOutDate,
			CreatedAt = DateTime.UtcNow
		};

	public static BookingModel ToBookingModel(this Booking booking) =>
		new()
		{
			Id = booking.Id,
			Status = booking.Status,
			TotalPrice = booking.TotalPrice,
			CreatedAt = booking.CreatedAt,
			CheckInDate = booking.CheckInDate,
			CheckOutDate = booking.CheckOutDate,
			Room = booking.Room.ToRoomModel(),
			Client = booking.Client.ToClientModel()
		};

	public static AddPaymentRequest ToAddPaymentRequest(this Booking booking) =>
		new()
		{
			BookingId = booking.Id,
			Amount = booking.TotalPrice
		};

	public static SendBookingNotificationRequest ToSendBookingNotificationRequest(this Booking booking) =>
		new()
		{
			BookingId = booking.Id,
			TotalPrice = booking.Room.Price,
			CheckInDate = booking.CheckInDate,
			CheckOutDate = booking.CheckOutDate,
			Client = booking.Client.ToClientModel(),
			Room = booking.Room.ToRoomNotificationRequest()
		};

	private static RoomNotificationRequest ToRoomNotificationRequest(this Room room) =>
		new()
		{
			Number = room.Number,
			Hotel = room.Hotel.ToHotelNotificationRequest()
		};

	private static HotelNotificationRequest ToHotelNotificationRequest(this Hotel hotel) =>
		new()
		{
			Name = hotel.Name,
			Address = hotel.Address,
			Country = hotel.Country
		};
}
