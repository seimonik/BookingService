namespace BookingService.Application.Models;

public class AuthClientModel
{
	public Guid? Id { get; set; }
	public string? FullName { get; set; }
	public string? Email { get; set; }
	public bool IsValid { get; set; }
}
