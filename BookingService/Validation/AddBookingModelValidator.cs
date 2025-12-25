using BookingService.Application.Models;
using BookingService.Dal;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Validation;

public class AddBookingModelValidator : AbstractValidator<AddBookingModel>
{
	private BookingServiceDbContext _dbContext;

	public AddBookingModelValidator(BookingServiceDbContext dbContext)
	{
		_dbContext = dbContext;

		//RuleFor(r => r.ClientId)
		//	.NotEmpty()
		//	.Must(CheckClient)
		//	.WithMessage("Клиент не существует в системе");
		//RuleFor(r => r.RoomId)
		//	.NotEmpty()
		//	.Must(CheckRoom)
		//	.WithMessage("Не найдена комната, пожалуйста, выберите другую");
		//RuleFor(r => r.CheckInDate).NotEmpty();
		//RuleFor(r => r.CheckOutDate).NotEmpty();
		RuleFor(x => x.CheckInDate)
			.NotEmpty()
			.Must(dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue) >= DateTime.Today)
			.WithMessage("Дата заселения введена некорректно");

		RuleFor(x => x.CheckOutDate)
			.NotEmpty()
			.GreaterThan(x => x.CheckInDate)
			.WithMessage("CheckOutDate должен быть позже CheckInDate");

		RuleFor(x => x.IdempotencyKey)
			.NotEmpty();
	}

	//private bool CheckClient(Guid clientId)
	//{
	//	return _dbContext.Clients.Any(x => x.Id == clientId);
	//}

	//private bool CheckRoom(Guid roomId)
	//{
	//	return _dbContext.Rooms.Any(x => x.Id == roomId);
	//}
}
