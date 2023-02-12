using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollingStationsResolver.Domain.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentityModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "ImportedPollingStations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    County = table.Column<string>(type: "text", nullable: false),
                    Locality = table.Column<string>(type: "text", nullable: false),
                    PollingStationNumber = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResolvedAddressStatus = table.Column<int>(type: "integer", nullable: false),
                    FailMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedPollingStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    Base64File = table.Column<string>(type: "text", nullable: false),
                    JobStatus = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PollingStations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    PollingStationNumber = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    County = table.Column<string>(type: "text", nullable: false),
                    Locality = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollingStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResolvedAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    County = table.Column<string>(type: "text", nullable: false),
                    Locality = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolvedAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportedPollingStationAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Locality = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: true),
                    StreetCode = table.Column<string>(type: "text", nullable: true),
                    HouseNumber = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    ImportedPollingStationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedPollingStationAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportedPollingStationAddresses_ImportedPollingStations_Imp~",
                        column: x => x.ImportedPollingStationId,
                        principalTable: "ImportedPollingStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PollingStationAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Locality = table.Column<string>(type: "text", nullable: false),
                    StreetCode = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    HouseNumbers = table.Column<string>(type: "text", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: false),
                    PollingStationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollingStationAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollingStationAddresses_PollingStations_PollingStationId",
                        column: x => x.PollingStationId,
                        principalTable: "PollingStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportedPollingStationAddresses_ImportedPollingStationId",
                table: "ImportedPollingStationAddresses",
                column: "ImportedPollingStationId");

            migrationBuilder.CreateIndex(
                name: "IX_PollingStationAddresses_PollingStationId",
                table: "PollingStationAddresses",
                column: "PollingStationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportedPollingStationAddresses");

            migrationBuilder.DropTable(
                name: "ImportJobs");

            migrationBuilder.DropTable(
                name: "PollingStationAddresses");

            migrationBuilder.DropTable(
                name: "ResolvedAddresses");

            migrationBuilder.DropTable(
                name: "ImportedPollingStations");

            migrationBuilder.DropTable(
                name: "PollingStations");
        }
    }
}
