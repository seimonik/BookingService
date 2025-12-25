namespace BookingService.Application.Models;

public class PromocodeValidationResult
{
	public bool IsValid { get; set; }
	public string? Message { get; set; }
	public decimal DiscountAmount { get; set; }
	public decimal FinalPrice { get; set; }
	public string? DiscountType { get; set; }
}