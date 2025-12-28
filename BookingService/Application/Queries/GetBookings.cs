using BookingService.Application.Models;
using BookingService.Dal;
using BookingService.Extensions.ModelConversion;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Application.Queries;

public static class GetBookings
{
	public record Query(Guid ClientId) : IRequest<IEnumerable<FullBookingModel>>;

	internal class Handler(BookingServiceDbContext dbContext) : IRequestHandler<Query, IEnumerable<FullBookingModel>>
	{
		public async Task<IEnumerable<FullBookingModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			var bookings = await dbContext.Bookings.AsNoTracking()
				.Include(x => x.Client)
				.Include(x => x.RoomType)
					.ThenInclude(x => x.Hotel)
				.Where(x => x.ClientId == request.ClientId &&
					x.Status != Dal.Enums.BookingStatus.Cancelled &&
					x.Status != Dal.Enums.BookingStatus.Pending)
				.OrderByDescending(x => x.CreatedAt)
				.ToListAsync(cancellationToken);

			return bookings.Select(x => x.ToFullBookingModel());
		}
	}
}