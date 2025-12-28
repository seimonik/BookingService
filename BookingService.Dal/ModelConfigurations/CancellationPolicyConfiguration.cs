using BookingService.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Dal.ModelConfigurations;

internal sealed class CancellationPolicyConfiguration : DalEntityTypeConfiguration<CancellationPolicy>
{
	public override void Configure(EntityTypeBuilder<CancellationPolicy> builder)
	{
		base.Configure(builder);

		builder.Property(x => x.PenaltyValue)
			.HasColumnType("decimal(18,2)");
	}
}