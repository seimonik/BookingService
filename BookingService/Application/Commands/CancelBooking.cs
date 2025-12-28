using BookingService.Application.Queries;
using BookingService.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Application.Commands;

public static class CancelBooking
{
	public record Command(Guid BookingId) : IRequest;

	internal class Handler(BookingServiceDbContext dbContext, IMediator mediator) : IRequestHandler<Command>
	{
		public async Task Handle(Command request, CancellationToken cancellationToken)
		{
			var booking = await dbContext.Bookings
				.FirstOrDefaultAsync(x => x.Id == request.BookingId, cancellationToken);

			var refundAmount = await mediator.Send(new CalculateRefundAmount.Query(request.BookingId), cancellationToken);
			// for test
			var card = await dbContext.Cards.FirstAsync(x => x.Id == booking!.CardId, cancellationToken);
			card.Balance += refundAmount;

			booking!.Status = Dal.Enums.BookingStatus.CustomCancellation;

			await dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}