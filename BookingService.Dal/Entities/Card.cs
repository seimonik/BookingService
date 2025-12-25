using BookingService.Dal.Interfaces;

namespace BookingService.Dal.Entities;

public class Card : IEntity
{
	public Guid Id { get; set; }
	public string CardNumber { get; set; } = string.Empty;
	public string Expiry { get; set; } = string.Empty;
	public string Cvv { get; set; } = string.Empty;
	public decimal Balance { get; set; } = 0;
}