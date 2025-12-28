using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyToCardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CardId",
                table: "Bookings",
                column: "CardId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Cards_CardId",
                table: "Bookings",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Cards_CardId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CardId",
                table: "Bookings");
        }
    }
}
