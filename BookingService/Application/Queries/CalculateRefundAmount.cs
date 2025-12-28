using BookingService.Dal;
using BookingService.Dal.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Application.Queries;

/// <summary>
/// Подсчет суммы для возврата
/// </summary>
public static class CalculateRefundAmount
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
				return 0; // Возврат 0, если невозвратный тариф

			var cancellationDate = DateOnly.FromDateTime(DateTime.Now);
			int daysToArrival = booking.CheckInDate.DayNumber - cancellationDate.DayNumber;

			// Если отменяют вовремя — возвращаем всё
			if (daysToArrival >= policy.FreeCancellationDays)
				return booking.TotalPrice;

			// Считаем сумму штрафа
			decimal penalty = policy.PenaltyType switch
			{
				PenaltyType.Percentage => booking.TotalPrice * (policy.PenaltyValue / 100),
				PenaltyType.FixedAmount => policy.PenaltyValue,
				PenaltyType.Nights => (booking.TotalPrice / (booking.CheckOutDate.DayNumber - booking.CheckInDate.DayNumber)) * policy.PenaltyValue,
				_ => 0
			};

			// Возвращаем остаток (Общая цена - штраф)
			return Math.Max(0, booking.TotalPrice - penalty);
		}
	}
}
