using BookingService.Dal;
using BookingService.Dal.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Application.Queries;

public static class CalculatePenalty
{
	public record Query(Guid BookingId) : IRequest<decimal>;

	internal class Handler(BookingServiceDbContext dbContext) : IRequestHandler<Query, decimal>
	{
		public async Task<decimal> Handle(Query request, CancellationToken cancellationToken)
		{
			var booking = await dbContext.Bookings.AsNoTracking()
				.Include(x => x.RoomType)
					.ThenInclude(x => x.CancellationPolicy)
				.FirstOrDefaultAsync(x => x.Id == request.BookingId, cancellationToken);

			var policy = booking!.RoomType.CancellationPolicy;
			if (policy == null || (policy.PenaltyType == PenaltyType.Percentage && policy.PenaltyValue == 100))
				return 0;

			// 1. Проверяем срок бесплатной отмены
			var cancellationDate = DateOnly.FromDateTime(DateTime.Now);
			int daysBeforeArrival = booking.CheckInDate.DayNumber - cancellationDate.DayNumber;
			if (daysBeforeArrival >= policy.FreeCancellationDays)
			{
				return 0;
			}

			decimal calculatedPenalty = 0;

			// 2. Считаем штраф согласно политике
			switch (policy.PenaltyType)
			{
				case PenaltyType.Nights:
					// Считаем стоимость одной ночи
					int totalNights = booking.CheckOutDate.DayNumber - booking.CheckInDate.DayNumber;
					decimal pricePerNight = booking.TotalPrice / totalNights;
					calculatedPenalty = pricePerNight * policy.PenaltyValue;
					break;

				case PenaltyType.FixedAmount:
					calculatedPenalty = policy.PenaltyValue;
					break;

				case PenaltyType.Percentage:
					calculatedPenalty = booking.TotalPrice * (policy.PenaltyValue / 100);
					break;
			}

			// 3. Штраф не может быть больше стоимости брони
			return Math.Min(calculatedPenalty, booking.TotalPrice);
		}
	}
}