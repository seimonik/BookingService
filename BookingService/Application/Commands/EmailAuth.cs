using BookingService.Dal;
using BookingService.Dal.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Application.Commands;

public static class EmailAuth
{
	public record Command(string Email) : IRequest;

	internal class Handler(BookingServiceDbContext dbContext, IMediator mediator) : IRequestHandler<Command>
	{
		public async Task Handle(Command request, CancellationToken cancellationToken)
		{
			// Удаляем старые коды
			await dbContext.EmailVerificationCodes
				.Where(c => c.Email == request.Email && !c.IsUsed)
				.ExecuteDeleteAsync(cancellationToken);

			var code = GenerateCode();
			var verificationCode = new EmailVerificationCode
			{
				Id = Guid.NewGuid(),
				Email = request.Email,
				Code = code,
				ExpiresAt = DateTime.UtcNow.AddMinutes(10),
				CreatedAt = DateTime.UtcNow,
				IsUsed = false
			};

			dbContext.EmailVerificationCodes.Add(verificationCode);
			await dbContext.SaveChangesAsync(cancellationToken);

			await mediator.Send(new SendEmail.Command(request.Email, code), cancellationToken);
		}

		private static string GenerateCode()
		{
			var random = new Random();
			return random.Next(100000, 999999).ToString();
		}
	}
}