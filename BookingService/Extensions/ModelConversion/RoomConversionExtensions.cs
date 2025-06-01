using BookingService.Application.Models;
using BookingService.Dal.Entities;

namespace BookingService.Extensions.ModelConversion;

public static class RoomConversionExtensions
{
	public static RoomModel ToRoomModel(this Room room) =>
		new()
		{
			Id = room.Id,
			Number = room.Number,
			Price = room.Price
		};
}
