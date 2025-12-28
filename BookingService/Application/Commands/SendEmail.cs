using BookingService.Application.Models;
using MediatR;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using Confluent.Kafka;

namespace BookingService.Application.Commands;

public static class SendEmail
{
	public record Command(string Email, string Code) : IRequest<Unit>;

	internal class Handler : IRequestHandler<Command, Unit>
	{
		public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
		{
			try
			{
				MailAddress from = new("bookingservicessu@mail.ru", "SSU Booking");
				MailAddress to = new(request.Email);

				MailMessage mailMessage = new(from, to)
				{
					Subject = "Код подтверждения бронирования",
					Body = $"""
                Ваш код подтверждения: <strong>{request.Code}</strong>
                Действует 10 минут.
                """,
					IsBodyHtml = true
				};

				SmtpClient smtp = new("smtp.mail.ru", 587)
				{
					Credentials = new NetworkCredential("bookingservicessu@mail.ru", "iuEpU1paqRVHxZWFJEdp"), // qjy7LdzqlbrWoIqcYXVR
					EnableSsl = true
				};
				await smtp.SendMailAsync(mailMessage, cancellationToken);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return Unit.Value;
		}
	}
}