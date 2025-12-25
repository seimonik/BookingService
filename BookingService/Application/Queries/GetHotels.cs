using BookingService.Application.Models;
using BookingService.Dal;
using BookingService.Dal.Entities;
using BookingService.Dal.Enums;
using BookingService.Extensions.ModelConversion;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Application.Queries;

public static class GetHotels
{
	public record Query(string? City = null, DateOnly? Start = null, DateOnly? End = null) : IRequest<IEnumerable<HotelModel>>;

	internal class Handler : IRequestHandler<Query, IEnumerable<HotelModel>>
	{
		private readonly BookingServiceDbContext _dbContext;

		public Handler(BookingServiceDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<HotelModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			List<Hotel> availableHotels = [];
			if (request.City != null)
			{
				availableHotels = await _dbContext.Hotels
					.AsNoTracking()
					.Include(x => x.RoomTypes)
					.Where(h => h.Address.Contains(request.City))
					.Where(h => h.RoomTypes != null && h.RoomTypes.Any(rt =>
						// Подзапрос: считаем количество броней для данного типа номера
						rt.TotalCount > _dbContext.Bookings.Count(b =>
							b.RoomTypeId == rt.Id &&
							b.Status != BookingStatus.Cancelled &&
							(request.End.HasValue ? b.CheckInDate < request.End : true) &&
							(request.Start.HasValue ? b.CheckOutDate > request.Start : true))))
					.ToListAsync();
			}
			else
			{
				availableHotels = await _dbContext.Hotels
					.Include(x => x.RoomTypes)
					.Where(h => h.RoomTypes != null && h.RoomTypes.Any())
					.AsNoTracking()
					.ToListAsync();
			}
			return availableHotels.Select(x => x.ToHotelModel()).ToList();
		}
	}
}
