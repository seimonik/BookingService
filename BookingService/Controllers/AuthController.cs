using BookingService.Application.Commands;
using BookingService.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookingService.Controllers;

[Route("api/auth")]
[ProducesResponseType(StatusCodes.Status200OK)]
public class AuthController(IMediator mediator) : ControllerBase
{
	[HttpPost("send-code")]
	public async Task<IActionResult> SendCode([FromQuery] string email, CancellationToken cancellationToken)
	{
		await mediator.Send(new EmailAuth.Command(email), cancellationToken);
		return Ok(new { message = "Код отправлен" });
	}

	[HttpPost("verify")]
	[SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AuthClientModel))]
	public async Task<IActionResult> VerifyCode([FromQuery] string email, [FromQuery] string code, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(new VerifyCode.Command(email, code), cancellationToken);

		if (!result.IsValid)
			return BadRequest("Неверный или просроченный код");

		return Ok(result);
	}
}