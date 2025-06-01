namespace BookingService.Helper;

public interface IProducer
{
	public Task ProduceMessage(string message, string messageType);
}
