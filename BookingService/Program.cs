using BookingService.Dal;
using BookingService.Dal.Extensions;
using BookingService.Helper;
using BookingService.Validation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var conffiguration = builder.Configuration;
var assembly = typeof(Program).Assembly;

var connectionString = conffiguration.GetConnectionString("BookingService");
var dbDataSource = BookingServiceDbContext.GetDataSource(connectionString!);
services.AddDbContext<BookingServiceDbContext>(opt => opt.UseNpgsql(dbDataSource));

// Add services to the container.

#pragma warning disable CS0618 // Тип или член устарел
services.AddControllers().AddFluentValidation(fv =>
{
	fv.RegisterValidatorsFromAssemblyContaining<AddBookingModelValidator>();
	fv.DisableDataAnnotationsValidation = true;
});
#pragma warning restore CS0618 // Тип или член устарел
services.AddHostedService<KafkaConsumerService>();
services.AddHostedService<OutboxProcessor>();
services.AddTransient<IProducer, Producer>();

services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.MapType<DateOnly>(() => new Microsoft.OpenApi.OpenApiSchema
	{
		Type = Microsoft.OpenApi.JsonSchemaType.String,
		Format = "date"
	});
});

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
		policy =>
		{
			policy.AllowAnyOrigin()
				  .AllowAnyHeader()
				  .AllowAnyMethod();
		});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.Services.ApplyMigration();

app.UseCors();
app.MapControllers();

app.Run();