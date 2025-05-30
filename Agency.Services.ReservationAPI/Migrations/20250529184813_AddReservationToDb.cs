using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agency.Services.ReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservationHeaders",
                columns: table => new
                {
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationHeaders", x => x.ReservationId);
                });

            migrationBuilder.CreateTable(
                name: "ReservationDetails",
                columns: table => new
                {
                    ReservationDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservationHeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationDetails", x => x.ReservationDetailsId);
                    table.ForeignKey(
                        name: "FK_ReservationDetails_ReservationHeaders_ReservationHeaderId",
                        column: x => x.ReservationHeaderId,
                        principalTable: "ReservationHeaders",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationDetails_ReservationHeaderId",
                table: "ReservationDetails",
                column: "ReservationHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationDetails");

            migrationBuilder.DropTable(
                name: "ReservationHeaders");
        }
    }
}
