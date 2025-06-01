using BookingService.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Dal.ModelConfigurations;

internal sealed class OutboxConfiguration : DalEntityTypeConfiguration<Outbox>
{
	public override void Configure(EntityTypeBuilder<Outbox> builder)
	{
		base.Configure(builder);

		builder.Property(b => b.CreatedAt)
			.ValueGeneratedNever()
			.HasDefaultValueSql("now() at time zone 'utc'");
	}
}
