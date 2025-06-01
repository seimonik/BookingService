using BookingService.Application.Commands;
using BookingService.Application.Models;
using Confluent.Kafka;
using MediatR;
using System.Text;
using System.Text.Json;

namespace BookingService.Helper;

public class KafkaConsumerService : BackgroundService
{
	const string HOST = "localhost:9092";
	const string TOPIC = "payment-service-add-booking";
	private readonly IServiceProvider _serviceProvider;

	public KafkaConsumerService(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		return Task.Run(async () =>
		{
			var consumerConfig = new ConsumerConfig
			{
				GroupId = "booking-consumer-group",
				BootstrapServers = HOST,
				SecurityProtocol = SecurityProtocol.Plaintext,
				AutoOffsetReset = AutoOffsetReset.Latest,
			};
			using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
			consumer.Subscribe(TOPIC);

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using var scope = _serviceProvider.CreateScope();
					var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

					var message = consumer.Consume(stoppingToken);
					// Обработка сообщения
					var messageType = message.Message.Headers.FirstOrDefault(x => x.Key == "Type")?.GetValueBytes();
					if (messageType != null && Encoding.UTF8.GetString(messageType) == "UpdateBookingStatusRequest")
					{
						Console.WriteLine($"<--- Start consume UpdateBookingStatusRequest: {message.Message.Value} --->");
						await mediator.Send(new UpdateBookingStatus.Command(
							JsonSerializer.Deserialize<UpdateBookingStatusRequest>(message.Message.Value)!));
					}
				}
				catch
				{
					Console.WriteLine("Consumer Error");
				}
			}
		}, stoppingToken);
	}
}
