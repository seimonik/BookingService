using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace BookingService.Dal.Extensions;

public static class ServiceCollectionExtensions
{
	private static void ReloadTypes(this DatabaseFacade database)
	{
		database.OpenConnection();
		var connection = database.GetDbConnection() as NpgsqlConnection;
		connection?.ReloadTypes();
	}

	public static IServiceProvider ApplyMigration(this IServiceProvider serviceProvider)
	{
		ArgumentNullException.ThrowIfNull(serviceProvider);

		using IServiceScope serviceScope = serviceProvider.CreateScope();
		using BookingServiceDbContext val = serviceScope.ServiceProvider.GetRequiredService<BookingServiceDbContext>();

		if (val.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
		{
			val.Database.Migrate();
			val.Database.ReloadTypes();
		}

		return serviceProvider;
	}
}
