using BookingService.Dal.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Dal.ModelConfigurations;

internal sealed class CardConfiguration : DalEntityTypeConfiguration<Card>
{
	public override void Configure(EntityTypeBuilder<Card> builder)
	{
		base.Configure(builder);
		builder.HasIndex(e => e.CardNumber).IsUnique();
	}
}
