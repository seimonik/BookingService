using BookingService.Dal;
using BookingService.Dal.Extensions;
using BookingService.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var conffiguration = builder.Configuration;
var assembly = typeof(Program).Assembly;

var connectionString = conffiguration.GetConnectionString("BookingService");
var dbDataSource = BookingServiceDbContext.GetDataSource(connectionString!);
services.AddDbContext<BookingServiceDbContext>(opt => opt.UseNpgsql(dbDataSource));

// Add services to the container.

services.AddControllers();
services.AddHostedService<KafkaConsumerService>();
services.AddHostedService<OutboxProcessor>();
services.AddTransient<IProducer, Producer>();

services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
	options.MapType<DateOnly>(() => new OpenApiSchema
	{
		Type = "string",
		Format = "date"
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.Services.ApplyMigration();

app.MapControllers();

app.Run();
