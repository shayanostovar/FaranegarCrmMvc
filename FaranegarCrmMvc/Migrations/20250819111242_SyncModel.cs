using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaranegarCrmMvc.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Direction = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    CallerIdNum = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CallerIdName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Src = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Dst = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    SrcChannel = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DstChannel = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnsweredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Disposition = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    HangupCause = table.Column<int>(type: "int", nullable: true),
                    HangupCauseText = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DurationSec = table.Column<int>(type: "int", nullable: true),
                    BillSec = table.Column<int>(type: "int", nullable: true),
                    RecordingFile = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: true),
                    RawJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CallLogs_StartAt",
                table: "CallLogs",
                column: "StartAt");

            migrationBuilder.CreateIndex(
                name: "IX_CallLogs_UniqueId",
                table: "CallLogs",
                column: "UniqueId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallLogs");
        }
    }
}
