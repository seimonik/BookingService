namespace BookingService.Dal.Enums;

public enum BookingStatus
{
	Pending = 0, // ожидание оплаты
	Confirmed = 1,
	Cancelled = 2, // отмена по причине неоплаты
	CustomCancellation = 3 // отмена пользователем
}
