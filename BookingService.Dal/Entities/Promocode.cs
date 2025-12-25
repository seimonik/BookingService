using BookingService.Dal.Enums;
using BookingService.Dal.Interfaces;

namespace BookingService.Dal.Entities;

public class Promocode : IEntity
{
	public Guid Id { get; set; }
	public string Code { get; set; } = "";
	public string? Description { get; set; }

	// Тип скидки: 0 - Процент (%), 1 - Фиксированная сумма (Руб)
	public DiscountType Type { get; set; }

	// Значение скидки (например, 10 для процентов или 500 для валюты)
	public decimal Value { get; set; }

	// Минимальная сумма бронирования, при которой код сработает
	public decimal? MinBookingAmount { get; set; }

	// Максимальный размер скидки (актуально для процентов, чтобы скидка не превысила лимит)
	public decimal? MaxDiscountAmount { get; set; }

	// Дата начала и окончания действия
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }

	// Лимиты использования
	public int MaxUsages { get; set; } // Общее кол-во (например, первые 100 человек)
	public int CurrentUsages { get; set; } // Сколько раз уже применили

	// Статус активности (позволяет быстро отключить код вручную)
	public bool IsActive { get; set; }
}
