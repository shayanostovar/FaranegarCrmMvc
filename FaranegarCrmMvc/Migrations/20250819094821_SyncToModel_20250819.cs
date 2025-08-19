using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaranegarCrmMvc.Migrations
{
    /// <inheritdoc />
    public partial class SyncToModel_20250819 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RowFingerprint",
                table: "SalesReports",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesReports_RowFingerprint",
                table: "SalesReports",
                column: "RowFingerprint",
                unique: true,
                filter: "[RowFingerprint] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SalesReports_RowFingerprint",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "RowFingerprint",
                table: "SalesReports");
        }
    }
}
