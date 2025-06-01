using BookingService.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Dal.ModelConfigurations;

internal sealed class BookingConfiguration : DalEntityTypeConfiguration<Booking>
{
	public override void Configure(EntityTypeBuilder<Booking> builder)
	{
		base.Configure(builder);

		builder.Property(b => b.CreatedAt)
			.ValueGeneratedNever()
			.HasDefaultValueSql("now() at time zone 'utc'");

		builder
			.HasOne(b => b.Room)
			.WithMany()
			.HasForeignKey(b => b.RoomId);

		builder
			.HasOne(b => b.Client)
			.WithMany()
			.HasForeignKey(b => b.ClientId);
	}
}
