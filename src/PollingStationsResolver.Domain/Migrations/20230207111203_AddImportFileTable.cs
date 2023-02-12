using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollingStationsResolver.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddImportFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportedPollingStationAddresses_ImportedPollingStations_Imp~",
                table: "ImportedPollingStationAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_PollingStationAddresses_PollingStations_PollingStationId",
                table: "PollingStationAddresses");

            migrationBuilder.DropIndex(
                name: "IX_ImportedPollingStationAddresses_ImportedPollingStationId",
                table: "ImportedPollingStationAddresses");

            migrationBuilder.DropColumn(
                name: "Base64File",
                table: "ImportJobs");

            migrationBuilder.DropColumn(
                name: "ImportedPollingStationId",
                table: "ImportedPollingStationAddresses");

            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "ImportedPollingStations",
                newName: "ImportJobId");

            migrationBuilder.RenameColumn(
                name: "HouseNumber",
                table: "ImportedPollingStationAddresses",
                newName: "HouseNumbers");

            migrationBuilder.AlterColumn<Guid>(
                name: "PollingStationId",
                table: "PollingStationAddresses",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "ImportedPollingStationId",
                table: "PollingStationAddresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "ImportJobs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ImportJobFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Base64File = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportJobFile", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PollingStationAddresses_ImportedPollingStationId",
                table: "PollingStationAddresses",
                column: "ImportedPollingStationId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportJobs_FileId",
                table: "ImportJobs",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedPollingStations_ImportJobId",
                table: "ImportedPollingStations",
                column: "ImportJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportedPollingStations_ImportJobs_ImportJobId",
                table: "ImportedPollingStations",
                column: "ImportJobId",
                principalTable: "ImportJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportJobs_ImportJobFile_FileId",
                table: "ImportJobs",
                column: "FileId",
                principalTable: "ImportJobFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PollingStationAddresses_ImportedPollingStations_ImportedPol~",
                table: "PollingStationAddresses",
                column: "ImportedPollingStationId",
                principalTable: "ImportedPollingStations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PollingStationAddresses_PollingStations_PollingStationId",
                table: "PollingStationAddresses",
                column: "PollingStationId",
                principalTable: "PollingStations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportedPollingStations_ImportJobs_ImportJobId",
                table: "ImportedPollingStations");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportJobs_ImportJobFile_FileId",
                table: "ImportJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_PollingStationAddresses_ImportedPollingStations_ImportedPol~",
                table: "PollingStationAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_PollingStationAddresses_PollingStations_PollingStationId",
                table: "PollingStationAddresses");

            migrationBuilder.DropTable(
                name: "ImportJobFile");

            migrationBuilder.DropIndex(
                name: "IX_PollingStationAddresses_ImportedPollingStationId",
                table: "PollingStationAddresses");

            migrationBuilder.DropIndex(
                name: "IX_ImportJobs_FileId",
                table: "ImportJobs");

            migrationBuilder.DropIndex(
                name: "IX_ImportedPollingStations_ImportJobId",
                table: "ImportedPollingStations");

            migrationBuilder.DropColumn(
                name: "ImportedPollingStationId",
                table: "PollingStationAddresses");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "ImportJobs");

            migrationBuilder.RenameColumn(
                name: "ImportJobId",
                table: "ImportedPollingStations",
                newName: "JobId");

            migrationBuilder.RenameColumn(
                name: "HouseNumbers",
                table: "ImportedPollingStationAddresses",
                newName: "HouseNumber");

            migrationBuilder.AlterColumn<Guid>(
                name: "PollingStationId",
                table: "PollingStationAddresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Base64File",
                table: "ImportJobs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ImportedPollingStationId",
                table: "ImportedPollingStationAddresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ImportedPollingStationAddresses_ImportedPollingStationId",
                table: "ImportedPollingStationAddresses",
                column: "ImportedPollingStationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportedPollingStationAddresses_ImportedPollingStations_Imp~",
                table: "ImportedPollingStationAddresses",
                column: "ImportedPollingStationId",
                principalTable: "ImportedPollingStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PollingStationAddresses_PollingStations_PollingStationId",
                table: "PollingStationAddresses",
                column: "PollingStationId",
                principalTable: "PollingStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
