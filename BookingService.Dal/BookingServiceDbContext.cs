using BookingService.Dal.Entities;
using BookingService.Dal.Enums;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.NameTranslation;

namespace BookingService.Dal;

public class BookingServiceDbContext : DbContext
{
	private static readonly INpgsqlNameTranslator _nullNameTranslator = new NpgsqlNullNameTranslator();

	public DbSet<Booking> Bookings { get; set; } = null!;
	public DbSet<Client> Clients { get; set; } = null!;
	public DbSet<Hotel> Hotels { get; set; } = null!;
	public DbSet<Room> Rooms { get; set; } = null!;
	public DbSet<Outbox> OutboxMessages { get; set; } = null!;
	public DbSet<RoomType> RoomTypes { get; set; } = null!;
	public DbSet<Promocode> Promocodes { get; set; } = null!;
	public DbSet<UsedPromocodes> UsedPromocodes { get; set; } = null!;
	public DbSet<Card> Cards { get; set; }


	public BookingServiceDbContext(DbContextOptions<BookingServiceDbContext> options) : base(options)
	{
	}

	public BookingServiceDbContext() { }

	public static NpgsqlDataSource GetDataSource(string connectionString)
	{
		var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

		dataSourceBuilder.MapEnum<BookingStatus>(pgName: "lookups.BookingStatus", nameTranslator: _nullNameTranslator);

		return dataSourceBuilder.Build();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingServiceDbContext).Assembly);

		modelBuilder.HasPostgresEnum<BookingStatus>(schema: "lookups", nameTranslator: _nullNameTranslator);
	}
}
