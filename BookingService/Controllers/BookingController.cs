using BookingService.Application.Commands;
using BookingService.Application.Models;
using BookingService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[Route("api/booking")]
[ProducesResponseType(StatusCodes.Status200OK)]
public class BookingController : ControllerBase
{
	private readonly IMediator _mediator;

	public BookingController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public Task<IEnumerable<HotelModel>> GetHotels() =>
		_mediator.Send(new GetHotels.Query());

	[HttpPost]
	public Task<BookingModel> AddBooking([FromBody] AddBookingModel request) =>
		_mediator.Send(new AddBooking.Command(request));
}
