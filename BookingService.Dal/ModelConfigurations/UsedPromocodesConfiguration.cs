using BookingService.Dal.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Dal.ModelConfigurations;

internal sealed class UsedPromocodesConfiguration : DalEntityTypeConfiguration<UsedPromocodes>
{
	public override void Configure(EntityTypeBuilder<UsedPromocodes> builder)
	{
		base.Configure(builder);

		builder.HasOne(up => up.Promocode)
			.WithMany()
			.HasForeignKey(up => up.PromocodeId);

		builder.HasOne(up => up.Client)
			.WithMany()
			.HasForeignKey(up => up.ClientId);
	}
}