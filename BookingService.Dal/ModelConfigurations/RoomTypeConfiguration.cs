using BookingService.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Dal.ModelConfigurations;

internal sealed class RoomTypeConfiguration : DalEntityTypeConfiguration<RoomType>
{
	public override void Configure(EntityTypeBuilder<RoomType> builder)
	{
		base.Configure(builder);

		builder
			.HasOne(r => r.CancellationPolicy)
			.WithMany(p => p.RoomTypes)
			.HasForeignKey(r => r.CancellationPolicyId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}
