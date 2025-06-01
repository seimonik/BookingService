using BookingService.Application.Models;
using BookingService.Dal.Entities;

namespace BookingService.Extensions.ModelConversion;

public static class HotelConversionExtensions
{
	public static HotelModel ToHotelModel(this Hotel hotel) =>
		new()
		{
			Id = hotel.Id,
			Name = hotel.Name,
			Address = hotel.Address,
			Country = hotel.Country,
			Rooms = hotel.Rooms
				.Where(x => x.IsAvailable)
				.Select(x => x.ToRoomModel()).ToArray()
		};
}
