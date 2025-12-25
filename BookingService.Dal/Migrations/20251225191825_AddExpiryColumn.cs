using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddExpiryColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Expiry",
                table: "Cards",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expiry",
                table: "Cards");
        }
    }
}
