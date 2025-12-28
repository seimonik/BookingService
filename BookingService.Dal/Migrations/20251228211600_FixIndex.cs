using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class FixIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_CardId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CardId",
                table: "Bookings",
                column: "CardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_CardId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CardId",
                table: "Bookings",
                column: "CardId",
                unique: true);
        }
    }
}
