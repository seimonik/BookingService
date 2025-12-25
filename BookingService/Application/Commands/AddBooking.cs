using BookingService.Application.Models;
using BookingService.Dal;
using BookingService.Dal.Entities;
using BookingService.Dal.Enums;
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
			var bookingByIdempotencyKey = await _dbContext.Bookings.AsNoTracking()
				.FirstOrDefaultAsync(x => x.IdempotencyKey == request.Request.IdempotencyKey, cancellationToken);

			if (bookingByIdempotencyKey != null)
				throw new InvalidOperationException("Пожалуйста, подождите, запрос уже создан и обрабатывается или повторите позже");

			using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

			// Считаем текущие брони на этот тип номера
			var bookedCount = await _dbContext.Bookings
				.CountAsync(b => b.RoomTypeId == request.Request.RoomTypeId &&
								 b.Status != BookingStatus.Cancelled &&
								 b.CheckInDate < request.Request.CheckOutDate &&
								 b.CheckOutDate > request.Request.CheckInDate,
							cancellationToken);

			var roomType = await _dbContext.RoomTypes
				.FirstAsync(rt => rt.Id == request.Request.RoomTypeId, cancellationToken);

			if (bookedCount >= roomType.TotalCount)
			{
				throw new InvalidOperationException("Все номера этого типа заняты на выбранные даты.");
			}

			// START Добавление клиента
			var client = await _dbContext.Clients.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Email == request.Request.Email, cancellationToken);
			if (client == null)
			{
				client = new Client
				{
					Id = Guid.NewGuid(),
					Email = request.Request.Email,
					FullName = request.Request.FullName
				};
				_dbContext.Clients.Add(client);
				await _dbContext.SaveChangesAsync(cancellationToken);
			}
			// END Добавление клиента

			// --- ЛОГИКА ПРОМОКОДА ---
			decimal finalPrice = roomType.Price;
			Promocode? appliedPromo = null;

			if (!string.IsNullOrWhiteSpace(request.Request.Promocode))
			{
				appliedPromo = await _dbContext.Promocodes
					.FirstOrDefaultAsync(p => p.Code == request.Request.Promocode.ToUpper(), cancellationToken);

				var now = DateTime.UtcNow;

				// Валидация
				if (appliedPromo == null || !appliedPromo.IsActive)
					throw new InvalidOperationException("Промокод не существует или неактивен.");

				if (now < appliedPromo.StartDate || now > appliedPromo.EndDate)
					throw new InvalidOperationException("Срок действия промокода истек.");

				if (appliedPromo.CurrentUsages >= appliedPromo.MaxUsages)
					throw new InvalidOperationException("Лимит использований промокода исчерпан.");

				if (roomType.Price < appliedPromo.MinBookingAmount)
					throw new InvalidOperationException($"Минимальная сумма для промокода: {appliedPromo.MinBookingAmount}");

				var alreadyUsed = await _dbContext.UsedPromocodes
					.AnyAsync(up => up.PromocodeId == appliedPromo.Id &&
					up.ClientId == client.Id,
					cancellationToken);
				if (alreadyUsed)
				{
					throw new InvalidOperationException("Вы уже использовали этот промокод ранее.");
				}

				// Расчет скидки
				decimal discount = appliedPromo.Type == DiscountType.Percent
					? (roomType.Price * appliedPromo.Value / 100)
					: appliedPromo.Value;

				// Проверка MaxDiscountAmount (если это проценты)
				if (appliedPromo.MaxDiscountAmount.HasValue && discount > appliedPromo.MaxDiscountAmount.Value)
					discount = appliedPromo.MaxDiscountAmount.Value;

				finalPrice = Math.Max(0, roomType.Price - discount);

				// Обновляем счетчик промокода
				appliedPromo.CurrentUsages++;
			}

			//var room = _dbContext.Rooms.FirstOrDefault(x => x.Id == request.Request.RoomId);
			var newBooking = request.Request.ToBooking(finalPrice, client.Id);
			if (appliedPromo != null)
			{
				_dbContext.UsedPromocodes.Add(new UsedPromocodes
				{
					PromocodeId = appliedPromo.Id,
					ClientId = client.Id,
					BookingId = newBooking.Id
				});
			}

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
				.Include(x => x.RoomType)
				.FirstOrDefault(x => x.Id == newBooking.Id);

			return booking!.ToBookingModel();
		}
	}
}