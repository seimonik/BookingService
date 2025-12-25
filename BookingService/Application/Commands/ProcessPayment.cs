using BookingService.Application.Models;
using BookingService.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Application.Commands;

public static class ProcessPayment
{
	public record Command(PaymentRequest Request) : IRequest<ProcessPaymentResult>;

	internal class Handler(BookingServiceDbContext dbContext) : IRequestHandler<Command, ProcessPaymentResult>
	{
		public async Task<ProcessPaymentResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (request.Request.Amount <= 0)
				return new ProcessPaymentResult { IsValid = false, Message = "Сумма должна быть > 0" };

			var card = await dbContext.Cards
				.FirstOrDefaultAsync(c => c.CardNumber == request.Request.CardNumber, cancellationToken: cancellationToken);

			if (card == null)
				return new ProcessPaymentResult { IsValid = false, Message = "Проверьте введенные данные и повторите попытку" };

			if (card.Balance < request.Request.Amount)
				return new ProcessPaymentResult { IsValid = false, Message = "Недостаточно средств" };

			card.Balance -= request.Request.Amount;
			await dbContext.SaveChangesAsync(cancellationToken);

			return new ProcessPaymentResult { IsValid = true, Message = "Оплата прошла." };
		}
	}
}