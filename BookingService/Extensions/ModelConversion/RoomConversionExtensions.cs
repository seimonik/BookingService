using BookingService.Application.Models;
using BookingService.Dal.Entities;

namespace BookingService.Extensions.ModelConversion;

public static class RoomConversionExtensions
{
	public static RoomTypeModel ToRoomTypeModel(this RoomType room) =>
		new()
		{
			Id = room.Id,
			Name = room.Name,
			TotalCount = room.TotalCount,
			Price = room.Price
		};

	public static FullRoomTypeModel ToFullRoomTypeModel(this RoomType room) =>
		new()
		{
			Id = room.Id,
			Name = room.Name,
			Price = room.Price,
			Hotel = room.Hotel.ToHotelMiniModel(),
			CancellationPolicy = room.CancellationPolicy?.ToCancellationPolicyModel()
		};
}
