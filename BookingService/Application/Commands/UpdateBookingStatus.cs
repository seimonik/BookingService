using BookingService.Application.Models;
using BookingService.Dal;
using BookingService.Extensions.ModelConversion;
using BookingService.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BookingService.Application.Commands;

public static class UpdateBookingStatus
{
	public record Command(UpdateBookingStatusRequest Request) : IRequest<Unit>;

	internal class Handler : IRequestHandler<Command, Unit>
	{
		public BookingServiceDbContext _dbContext;
		public IProducer _producer;

		public Handler(BookingServiceDbContext dbContext, IProducer producer)
		{
			_dbContext = dbContext;
			_producer = producer;
		}

		public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
		{
			var booking = await _dbContext.Bookings
				.Include(x => x.Room)
					.ThenInclude(r => r.Hotel)
				.Include(x => x.Client)
				.FirstOrDefaultAsync(x => x.Id == request.Request.BookingId);

			if (booking != null)
			{
				booking.Status = request.Request.Status;
				await _dbContext.SaveChangesAsync(cancellationToken);

				if (booking.Status == Dal.Enums.BookingStatus.Confirmed)
					await _producer.ProduceMessage(JsonSerializer.Serialize(booking!.ToSendBookingNotificationRequest()), "SendBookingNotificationRequest");
			}
			return Unit.Value;
		}
	}
}
