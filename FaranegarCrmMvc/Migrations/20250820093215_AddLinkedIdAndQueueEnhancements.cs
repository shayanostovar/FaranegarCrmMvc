using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaranegarCrmMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkedIdAndQueueEnhancements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkedId",
                table: "QueueLogs",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedId",
                table: "CallLogs",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QueueLogs_LinkedId",
                table: "QueueLogs",
                column: "LinkedId");

            migrationBuilder.CreateIndex(
                name: "IX_CallLogs_LinkedId",
                table: "CallLogs",
                column: "LinkedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QueueLogs_LinkedId",
                table: "QueueLogs");

            migrationBuilder.DropIndex(
                name: "IX_CallLogs_LinkedId",
                table: "CallLogs");

            migrationBuilder.DropColumn(
                name: "LinkedId",
                table: "QueueLogs");

            migrationBuilder.DropColumn(
                name: "LinkedId",
                table: "CallLogs");
        }
    }
}
