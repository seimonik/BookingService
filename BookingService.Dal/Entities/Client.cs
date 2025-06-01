using BookingService.Dal.Interfaces;

namespace BookingService.Dal.Entities;

public class Client : IEntity
{
	public Guid Id { get; set; }
	public string FullName { get; set; } = "";
	public string Email { get; set; } = "";
}
