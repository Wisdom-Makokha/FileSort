using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileSort.Migrations
{
    /// <inheritdoc />
    public partial class InstanceIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ApplicationInstances_InitiationTime",
                table: "ApplicationInstances",
                column: "InitiationTime",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationInstances_InitiationTime",
                table: "ApplicationInstances");
        }
    }
}
