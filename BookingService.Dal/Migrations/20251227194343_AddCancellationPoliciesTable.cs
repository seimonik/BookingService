using System;
using BookingService.Dal.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddCancellationPoliciesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:lookups.BookingStatus", "Pending,Confirmed,Cancelled")
                .Annotation("Npgsql:Enum:lookups.PenaltyType", "Percentage,FixedAmount,Nights")
                .OldAnnotation("Npgsql:Enum:lookups.BookingStatus", "Pending,Confirmed,Cancelled");

            migrationBuilder.AddColumn<Guid>(
                name: "CancellationPolicyId",
                table: "RoomTypes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CancellationPolicies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FreeCancellationDays = table.Column<int>(type: "integer", nullable: false),
                    PenaltyType = table.Column<PenaltyType>(type: "lookups.\"PenaltyType\"", nullable: false),
                    PenaltyValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancellationPolicies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_CancellationPolicyId",
                table: "RoomTypes",
                column: "CancellationPolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTypes_CancellationPolicies_CancellationPolicyId",
                table: "RoomTypes",
                column: "CancellationPolicyId",
                principalTable: "CancellationPolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomTypes_CancellationPolicies_CancellationPolicyId",
                table: "RoomTypes");

            migrationBuilder.DropTable(
                name: "CancellationPolicies");

            migrationBuilder.DropIndex(
                name: "IX_RoomTypes_CancellationPolicyId",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "CancellationPolicyId",
                table: "RoomTypes");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:lookups.BookingStatus", "Pending,Confirmed,Cancelled")
                .OldAnnotation("Npgsql:Enum:lookups.BookingStatus", "Pending,Confirmed,Cancelled")
                .OldAnnotation("Npgsql:Enum:lookups.PenaltyType", "Percentage,FixedAmount,Nights");
        }
    }
}
