using BookingService.Dal;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Helper;

public class OutboxProcessor : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly IProducer _producer;

	public OutboxProcessor(IServiceProvider serviceProvider, IProducer producer)
	{
		_serviceProvider = serviceProvider;
		_producer = producer;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			using var scope = _serviceProvider.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<BookingServiceDbContext>();
			var messages = await dbContext.OutboxMessages
				.Where(m => !m.Processed)
				.OrderBy(m => m.CreatedAt)
				.ToListAsync(stoppingToken);

			foreach (var message in messages)
			{
				try
				{
					await _producer.ProduceMessage(message.Payload, message.Type);

					message.Processed = true;
					await dbContext.SaveChangesAsync(stoppingToken);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Произошла ошибка при попытке отправить сообщение в брокер: {ex.Message}");
				}
			}

			await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
		}
	}
}
