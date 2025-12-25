using BookingService.Dal.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Dal.ModelConfigurations;

internal sealed class HotelConfiguration : DalEntityTypeConfiguration<Hotel>
{
	public override void Configure(EntityTypeBuilder<Hotel> builder)
	{
		base.Configure(builder);

		builder
			.HasMany(h => h.Rooms)
			.WithOne(r => r.Hotel)
			.HasForeignKey(r => r.HotelId);

		builder
			.HasMany(h => h.RoomTypes)
			.WithOne(r => r.Hotel)
			.HasForeignKey(r => r.HotelId);
	}
}
