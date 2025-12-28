using BookingService.Dal.Interfaces;

namespace BookingService.Dal.Entities;

public class EmailVerificationCode : IEntity
{
	public Guid Id { get; set; }
	public string Email { get; set; } = "";
	public string Code { get; set; } = ""; // 6-значный код
	public DateTime ExpiresAt { get; set; }
	public DateTime CreatedAt { get; set; }
	public bool IsUsed { get; set; }
}