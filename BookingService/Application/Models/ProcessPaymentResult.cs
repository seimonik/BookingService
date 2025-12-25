namespace BookingService.Application.Models;

public class ProcessPaymentResult
{
	public bool IsValid { get; set; }
	public string Message { get; set; } = "";
}
