using BookingService.Dal.Enums;
using BookingService.Dal.Interfaces;

namespace BookingService.Dal.Entities;

public class CancellationPolicy : IEntity
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;

	// За сколько дней до заезда можно отменить без штрафа
	public int FreeCancellationDays { get; set; }

	// Тип штрафа (процент, сумма или ночи)
	public PenaltyType PenaltyType { get; set; }

	// Значение (например, 100 для процента или 5000 для суммы) Для невозвратного тарифа - 100%
	public decimal PenaltyValue { get; set; }

	// Флаг невозвратного тарифа (штраф 100% всегда)
	//public bool IsNonRefundable { get; set; }

	public ICollection<RoomType> RoomTypes { get; set; } = [];
}
