using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HotelId1",
                table: "Rooms",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HotelId1",
                table: "Rooms",
                column: "HotelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Hotels_HotelId1",
                table: "Rooms",
                column: "HotelId1",
                principalTable: "Hotels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Hotels_HotelId1",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_HotelId1",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "HotelId1",
                table: "Rooms");
        }
    }
}
