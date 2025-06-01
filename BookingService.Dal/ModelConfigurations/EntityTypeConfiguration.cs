using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Dal.ModelConfigurations;

public abstract class EntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : class
{
	public abstract void Configure(EntityTypeBuilder<T> builder);
}
