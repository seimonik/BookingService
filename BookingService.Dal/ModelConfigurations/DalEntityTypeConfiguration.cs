using BookingService.Dal.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Dal.ModelConfigurations;

public abstract class DalEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class, IEntity
{
	public override void Configure(EntityTypeBuilder<T> builder)
	{
		builder.HasKey((T x) => x.Id); 
	}
}
