using BookingService.Dal.Entities;
using BookingService.Dal.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Dal.ModelConfigurations;

internal sealed class EmailVerificationCodeConfiguration : DalEntityTypeConfiguration<EmailVerificationCode>
{
	public override void Configure(EntityTypeBuilder<EmailVerificationCode> builder)
	{
		base.Configure(builder);

		builder.HasIndex(e => e.Email);
		builder.HasIndex(e => new { e.Email, e.IsUsed });
	}
}