using BookingService.Application.Models;
using BookingService.Dal;
using BookingService.Dal.Entities;
using BookingService.Extensions.ModelConversion;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BookingService.Application.Commands;

public static class AddBooking
{
	public record Command(AddBookingModel Request) : IRequest<BookingModel>;

	internal class Handler : IRequestHandler<Command, BookingModel>
	{
		public BookingServiceDbContext _dbContext;

		public Handler(BookingServiceDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<BookingModel> Handle(Command request, CancellationToken cancellationToken)
		{
			using var transaction = await _dbContext.Database.BeginTransactionAsync();

			var room = _dbContext.Rooms.FirstOrDefault(x => x.Id == request.Request.RoomId);
			var newBooking = request.Request.ToBooking(room!.Price);

			var outboxMessage = new Outbox
			{
				CreatedAt = DateTime.UtcNow,
				Type = nameof(AddPaymentRequest),
				Payload = JsonSerializer.Serialize(newBooking!.ToAddPaymentRequest())
			};

			_dbContext.OutboxMessages.Add(outboxMessage);
			_dbContext.Bookings.Add(newBooking);
			await _dbContext.SaveChangesAsync(cancellationToken);
			await transaction.CommitAsync(cancellationToken);

			var booking = _dbContext.Bookings.AsNoTracking()
				.Include(x => x.Client)
				.Include(x => x.Room)
				.FirstOrDefault(x => x.Id == newBooking.Id);

			return booking!.ToBookingModel();
		}
	}
}
