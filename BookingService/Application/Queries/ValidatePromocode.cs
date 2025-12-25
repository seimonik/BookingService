using BookingService.Application.Models;
using BookingService.Dal;
using BookingService.Dal.Entities;
using BookingService.Dal.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Application.Queries;

public static class ValidatePromocode
{
	public record Query(
		string Code,
		Guid RoomTypeId,
		string Email,
		string FullName) : IRequest<PromocodeValidationResult>;

	internal class Handler(BookingServiceDbContext dbContext) : IRequestHandler<Query, PromocodeValidationResult>
	{
		public async Task<PromocodeValidationResult> Handle(Query request, CancellationToken cancellationToken)
		{
			var roomType = await dbContext.RoomTypes.AsNoTracking()
				.FirstOrDefaultAsync(rt => rt.Id == request.RoomTypeId, cancellationToken);

			if (roomType == null)
				return new PromocodeValidationResult
				{
					IsValid = false,
					Message = "Тип номера не найден",
					DiscountAmount = 0,
					FinalPrice = 0
				};

			if (string.IsNullOrEmpty(request.FullName) || string.IsNullOrEmpty(request.Email))
			{
				return new PromocodeValidationResult
				{
					IsValid = false,
					Message = "Сначала заполните данные о покупателе",
					DiscountAmount = 0,
					FinalPrice = roomType.Price
				};
			}

			var client = await dbContext.Clients.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
			if (client == null)
			{
				client = new Client
				{
					Id = Guid.NewGuid(),
					Email = request.Email,
					FullName = request.FullName
				};
				dbContext.Clients.Add(client);
				await dbContext.SaveChangesAsync(cancellationToken);
			}

			var promo = await dbContext.Promocodes
				.FirstOrDefaultAsync(p => p.Code == request.Code.ToUpper(), cancellationToken);

			// 1. Базовые проверки
			if (promo == null)
				return new PromocodeValidationResult
				{
					IsValid = false,
					Message = "Промокод не существует",
					DiscountAmount = 0,
					FinalPrice = roomType.Price
				};

			if (!promo.IsActive)
				return new PromocodeValidationResult
				{
					IsValid = false,
					Message = "Промокод деактивирован",
					DiscountAmount = 0,
					FinalPrice = roomType.Price
				};

			if (DateTime.UtcNow < promo.StartDate || DateTime.UtcNow > promo.EndDate)
				return new PromocodeValidationResult
				{
					IsValid = false,
					Message = "Срок действия промокода истек",
					DiscountAmount = 0,
					FinalPrice = roomType.Price
				};

			if (promo.CurrentUsages >= promo.MaxUsages)
				return new PromocodeValidationResult
				{
					IsValid = false,
					Message = "Лимит использований промокода исчерпан",
					DiscountAmount = 0,
					FinalPrice = roomType.Price
				};

			if (roomType.Price < promo.MinBookingAmount)
				return new PromocodeValidationResult
				{
					IsValid = false,
					Message = $"Минимальная сумма для активации: {promo.MinBookingAmount}",
					DiscountAmount = 0,
					FinalPrice = roomType.Price
				};

			// 2. Проверка "в одни руки"
			var alreadyUsed = await dbContext.UsedPromocodes
				.AnyAsync(up => up.PromocodeId == promo.Id && up.ClientId == client.Id, cancellationToken);

			if (alreadyUsed)
				return new PromocodeValidationResult
				{
					IsValid = false,
					Message = "Вы уже использовали этот промокод",
					DiscountAmount = 0,
					FinalPrice = roomType.Price
				};

			// 3. Расчет скидки
			decimal discount = promo.Type == DiscountType.Percent
				? (roomType.Price * promo.Value / 100)
				: promo.Value;

			if (promo.MaxDiscountAmount.HasValue && discount > promo.MaxDiscountAmount.Value)
				discount = promo.MaxDiscountAmount.Value;

			return new PromocodeValidationResult
			{
				IsValid = true,
				Message = "Промокод применен",
				DiscountAmount = discount,
				FinalPrice = Math.Max(0, roomType.Price - discount),
				DiscountType = promo.Type.ToString()
			};
		}
	}
}