using BookingService.Application.Models;
using BookingService.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Application.Commands;

public static class VerifyCode
{
	public record Command(string Email, string Code) : IRequest<AuthClientModel>;

	internal class Handler(BookingServiceDbContext dbContext) : IRequestHandler<Command, AuthClientModel>
	{
		public async Task<AuthClientModel> Handle(Command request, CancellationToken cancellationToken)
		{
			var verificationCode = await dbContext.EmailVerificationCodes
				.FirstOrDefaultAsync(c => c.Email == request.Email
				&& c.Code == request.Code
				&& !c.IsUsed
				&& c.ExpiresAt > DateTime.UtcNow, cancellationToken);

			if (verificationCode == null)
				return new() { IsValid = false };

			verificationCode.IsUsed = true;
			await dbContext.SaveChangesAsync(cancellationToken);

			var client = await dbContext.Clients.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

			return new()
			{
				IsValid = true,
				Email = request.Email,
				FullName = client?.FullName,
				Id = client?.Id
			};
		}
	}
}