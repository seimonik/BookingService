using BookingService.Application.Models;
using BookingService.Dal;
using BookingService.Extensions.ModelConversion;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Application.Queries;

public static class GetHotel
{
	public record Query(Guid HotelId) : IRequest<HotelModel?>;

	internal class Handler(BookingServiceDbContext dbContext) : IRequestHandler<Query, HotelModel?>
	{
		public async Task<HotelModel?> Handle(Query request, CancellationToken cancellationToken) =>
			(await dbContext.Hotels.AsNoTracking()
				.Include(x => x.RoomTypes)
				.FirstOrDefaultAsync(x => x.Id == request.HotelId, cancellationToken))?
				.ToHotelModel();
	}
}
