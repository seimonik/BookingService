using BookingService.Dal.Interfaces;

namespace BookingService.Dal.Entities;

public class Outbox : IEntity
{
	public Guid Id { get; set; }
	public string Type { get; set; } = "";
	public string Payload { get; set; } = "";
	public DateTime CreatedAt { get; set; }
	public bool Processed { get; set; }
}
