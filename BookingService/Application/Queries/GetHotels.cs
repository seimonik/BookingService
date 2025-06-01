using BookingService.Application.Models;
using BookingService.Dal;
using BookingService.Extensions.ModelConversion;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Application.Queries;

public class GetHotels
{
	public record Query() : IRequest<IEnumerable<HotelModel>>;

	internal class Handler : IRequestHandler<Query, IEnumerable<HotelModel>>
	{
		private readonly BookingServiceDbContext _dbContext;

		public Handler(BookingServiceDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<HotelModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			var hotels = await _dbContext.Hotels
				.Include(x => x.Rooms)
				.AsNoTracking()
				.ToListAsync();

			return hotels.Select(x => x.ToHotelModel()).ToList();
		}
	}
}
