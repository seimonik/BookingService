using BookingService.Dal.Interfaces;

namespace BookingService.Dal.Entities;

public class UsedPromocodes : IEntity
{
	public Guid Id { get; set; }
	public Guid PromocodeId { get; set; }
	public Guid ClientId { get; set; }
	public Guid BookingId { get; set; }

	public Promocode Promocode { get; set; } = null!;
	public Client Client { get; set; } = null!;
}
