using BookingService.Dal.Enums;

namespace BookingService.Application.Models;

public class CancellationPolicyModel
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;

	// За сколько дней до заезда можно отменить без штрафа
	public int FreeCancellationDays { get; set; }

	// Тип штрафа (процент, сумма или ночи)
	public PenaltyType PenaltyType { get; set; }

	// Значение (например, 100 для процента или 5000 для суммы) Для невозвратного тарифа - 100%
	public decimal PenaltyValue { get; set; }
}
