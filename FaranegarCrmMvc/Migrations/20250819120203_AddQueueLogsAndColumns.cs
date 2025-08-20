using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaranegarCrmMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddQueueLogsAndColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgentChannel",
                table: "CallLogs",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AgentConnectAt",
                table: "CallLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgentExt",
                table: "CallLogs",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QueueHoldSec",
                table: "CallLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QueueJoinAt",
                table: "CallLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QueueLeaveAt",
                table: "CallLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QueueName",
                table: "CallLogs",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RingSec",
                table: "CallLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TalkSec",
                table: "CallLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QueueLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Event = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Queue = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Position = table.Column<int>(type: "int", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: true),
                    CallerIdNum = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    MemberName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Interface = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    AgentChannel = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    HoldTime = table.Column<int>(type: "int", nullable: true),
                    RingTime = table.Column<int>(type: "int", nullable: true),
                    TalkTime = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RawJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QueueLogs_OccurredAt",
                table: "QueueLogs",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_QueueLogs_UniqueId",
                table: "QueueLogs",
                column: "UniqueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QueueLogs");

            migrationBuilder.DropColumn(
                name: "AgentChannel",
                table: "CallLogs");

            migrationBuilder.DropColumn(
                name: "AgentConnectAt",
                table: "CallLogs");

            migrationBuilder.DropColumn(
                name: "AgentExt",
                table: "CallLogs");

            migrationBuilder.DropColumn(
                name: "QueueHoldSec",
                table: "CallLogs");

            migrationBuilder.DropColumn(
                name: "QueueJoinAt",
                table: "CallLogs");

            migrationBuilder.DropColumn(
                name: "QueueLeaveAt",
                table: "CallLogs");

            migrationBuilder.DropColumn(
                name: "QueueName",
                table: "CallLogs");

            migrationBuilder.DropColumn(
                name: "RingSec",
                table: "CallLogs");

            migrationBuilder.DropColumn(
                name: "TalkSec",
                table: "CallLogs");
        }
    }
}
