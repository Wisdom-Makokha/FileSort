using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileSort.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationInstances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationInstanceId",
                table: "Files",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ApplicationInstances",
                columns: table => new
                {
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitiationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ClosingTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationInstances", x => x.ApplicationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_ApplicationInstanceId",
                table: "Files",
                column: "ApplicationInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_ApplicationInstances_ApplicationInstanceId",
                table: "Files",
                column: "ApplicationInstanceId",
                principalTable: "ApplicationInstances",
                principalColumn: "ApplicationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_ApplicationInstances_ApplicationInstanceId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "ApplicationInstances");

            migrationBuilder.DropIndex(
                name: "IX_Files_ApplicationInstanceId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ApplicationInstanceId",
                table: "Files");
        }
    }
}
