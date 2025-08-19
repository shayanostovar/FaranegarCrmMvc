using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaranegarCrmMvc.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesReports_Accounts_AccountId",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "CommissionAmount",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "CommissionPercent",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "Credit",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "Debit",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "DocDate",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "FareAmount",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "PassengerName",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "ReceiveDate",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "Route",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "SalesReports");

            migrationBuilder.AlterColumn<string>(
                name: "TicketNumber",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PNR",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Airline",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReports_Accounts_AccountId",
                table: "SalesReports",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesReports_Accounts_AccountId",
                table: "SalesReports");

            migrationBuilder.AlterColumn<string>(
                name: "TicketNumber",
                table: "SalesReports",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PNR",
                table: "SalesReports",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Airline",
                table: "SalesReports",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionAmount",
                table: "SalesReports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionPercent",
                table: "SalesReports",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Credit",
                table: "SalesReports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Debit",
                table: "SalesReports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DocDate",
                table: "SalesReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FareAmount",
                table: "SalesReports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassengerName",
                table: "SalesReports",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "SalesReports",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiveDate",
                table: "SalesReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Route",
                table: "SalesReports",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "SalesReports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReports_Accounts_AccountId",
                table: "SalesReports",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
