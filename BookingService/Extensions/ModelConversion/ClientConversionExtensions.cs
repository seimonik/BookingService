using BookingService.Application.Models;
using BookingService.Dal.Entities;

namespace BookingService.Extensions.ModelConversion;

public static class ClientConversionExtensions
{
	public static ClientModel ToClientModel(this Client client) =>
		new()
		{
			FullName = client.FullName,
			Email = client.Email
		};
}
