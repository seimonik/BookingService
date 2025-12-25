namespace BookingService.Application.Models;

public class PaymentRequest
{
	public decimal Amount { get; set; }
	public string CardNumber { get; set; } = "";
	public string Expiry { get; set; } = "";
	public string Cvv { get; set; } = "";
}