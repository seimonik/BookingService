using BookingService.Application.Models;
using BookingService.Dal.Entities;

namespace BookingService.Extensions.ModelConversion;

public static class BookingConversionExtensions
{
	public static Booking ToBooking(this AddBookingModel bookingModel, decimal totalPrice, Guid clientId) =>
		new()
		{
			Id = Guid.NewGuid(),
			ClientId = clientId,
			RoomTypeId = bookingModel.RoomTypeId,
			//RoomId = bookingModel.RoomId,
			Status = Dal.Enums.BookingStatus.Pending,
			TotalPrice = totalPrice,
			CheckInDate = bookingModel.CheckInDate,
			CheckOutDate = bookingModel.CheckOutDate,
			CreatedAt = DateTime.UtcNow,
			IdempotencyKey = bookingModel.IdempotencyKey
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
			Room = booking.RoomType.ToRoomTypeModel(),
			Client = booking.Client.ToClientModel()
		};

	public static FullBookingModel ToFullBookingModel(this Booking booking) =>
		new()
		{
			Id = booking.Id,
			Status = booking.Status,
			TotalPrice = booking.TotalPrice,
			CreatedAt = booking.CreatedAt,
			CheckInDate = booking.CheckInDate,
			CheckOutDate = booking.CheckOutDate,
			RoomType = booking.RoomType.ToFullRoomTypeModel()
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
			TotalPrice = booking.RoomType.Price,
			CheckInDate = booking.CheckInDate,
			CheckOutDate = booking.CheckOutDate,
			Client = booking.Client.ToClientModel(),
			RoomType = booking.RoomType.ToRoomNotificationRequest()
		};

	private static RoomNotificationRequest ToRoomNotificationRequest(this RoomType room) =>
		new()
		{
			//Number = room.Number,
			TypeName = room.Name,
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
