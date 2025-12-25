using BookingService.Application.Commands;
using BookingService.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookingService.Controllers;

[Route("api/payment")]
[ProducesResponseType(StatusCodes.Status200OK)]
public class PaymentsController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	[SwaggerOperation(OperationId = nameof(ProcessPayment))]
	public Task<ProcessPaymentResult> ProcessPayment([FromBody] PaymentRequest request, CancellationToken cancellationToken) =>
		mediator.Send(new ProcessPayment.Command(request), cancellationToken);
}