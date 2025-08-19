using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaranegarCrmMvc.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    InvoiceRef = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Airline = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    TicketNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    PNR = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PassengerName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Route = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    FareAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CommissionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CommissionPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdditionalJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImportedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesReports_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Code",
                table: "Accounts",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReports_AccountId",
                table: "SalesReports",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReports_InvoiceRef",
                table: "SalesReports",
                column: "InvoiceRef");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesReports");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
