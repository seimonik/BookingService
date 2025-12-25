using BookingService.Application.Commands;
using BookingService.Application.Models;
using BookingService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
	[SwaggerOperation(OperationId = nameof(GetHotels))]
	public Task<IEnumerable<HotelModel>> GetHotels() =>
		_mediator.Send(new GetHotels.Query());

	[HttpGet("{hotelId}")]
	[SwaggerOperation(OperationId = nameof(GetHotel))]
	public Task<HotelModel?> GetHotel(Guid hotelId, CancellationToken cancellationToken) =>
		_mediator.Send(new GetHotel.Query(hotelId), cancellationToken);

	[HttpPost]
	[SwaggerOperation(OperationId = nameof(AddBooking))]
	[SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BookingModel))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> AddBooking([FromBody] AddBookingModel request)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		try
		{
			var result = await _mediator.Send(new AddBooking.Command(request));
			return Ok(result);
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpGet("validate-promo")]
	[SwaggerOperation(OperationId = nameof(ValidatePromocode))]
	public Task<PromocodeValidationResult> ValidatePromocode(
		[FromQuery] string code,
		[FromQuery] Guid roomTypeId,
		[FromQuery] string email,
		[FromQuery] string fullName,
		CancellationToken cancellationToken) =>
		_mediator.Send(new ValidatePromocode.Query(code, roomTypeId, email, fullName), cancellationToken);
}