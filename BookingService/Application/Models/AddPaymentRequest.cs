namespace BookingService.Application.Models;

public class AddPaymentRequest
{
	public Guid BookingId { get; set; }
	public decimal Amount { get; set; }
}
