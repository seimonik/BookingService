using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomCancellationBookingStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:lookups.BookingStatus", "Pending,Confirmed,Cancelled,CustomCancellation")
                .Annotation("Npgsql:Enum:lookups.PenaltyType", "Percentage,FixedAmount,Nights")
                .OldAnnotation("Npgsql:Enum:lookups.BookingStatus", "Pending,Confirmed,Cancelled")
                .OldAnnotation("Npgsql:Enum:lookups.PenaltyType", "Percentage,FixedAmount,Nights");

            migrationBuilder.AddColumn<Guid>(
                name: "CardId",
                table: "Bookings",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Bookings");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:lookups.BookingStatus", "Pending,Confirmed,Cancelled")
                .Annotation("Npgsql:Enum:lookups.PenaltyType", "Percentage,FixedAmount,Nights")
                .OldAnnotation("Npgsql:Enum:lookups.BookingStatus", "Pending,Confirmed,Cancelled,CustomCancellation")
                .OldAnnotation("Npgsql:Enum:lookups.PenaltyType", "Percentage,FixedAmount,Nights");
        }
    }
}
