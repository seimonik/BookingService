using BookingService.Application.Models;
using BookingService.Dal.Entities;

namespace BookingService.Extensions.ModelConversion;

public static class CancellationPolicyExtensions
{
	public static CancellationPolicyModel ToCancellationPolicyModel(this CancellationPolicy entity) =>
		new()
		{
			Id = entity.Id,
			Name = entity.Name,
			FreeCancellationDays = entity.FreeCancellationDays,
			PenaltyType = entity.PenaltyType,
			PenaltyValue = entity.PenaltyValue
		};
}