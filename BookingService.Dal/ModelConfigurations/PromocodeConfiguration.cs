using BookingService.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Dal.ModelConfigurations;

internal sealed class PromocodeConfiguration : DalEntityTypeConfiguration<Promocode>
{
	public override void Configure(EntityTypeBuilder<Promocode> builder)
	{
		base.Configure(builder);

		builder.Property(p => p.Code)
			.IsRequired();

		builder.Property(p => p.CurrentUsages)
			.HasDefaultValue(0);

		builder.Property(p => p.IsActive)
			.HasDefaultValue(true);
	}
}