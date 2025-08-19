using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaranegarCrmMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddExactFaranegarColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "-",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FARE",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "I6",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KU",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LP",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PNR1",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sign",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TAX",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TOTAL",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tax قبلی",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "V0",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ایرلاین",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "بدهکار",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "بستانکار",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "تاریخ",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "تاریخ برگشت",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "تاریخ دریافت",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "تاریخ رفت",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "تاریخ سند",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "تایید حسابداری",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "تخفیف",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ترتیب",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "تصویر بلیت",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "توضیحات",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "توضیحات فاکتور",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "جریمه",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "خالص Fare",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "خط بعد",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "درخواست کننده",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "درصد کمیسیون",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "سن",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ش ک فاکتور",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "شماره بلیط",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "شماره بلیط قبلی",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "شماره سند",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "شماره قرارداد",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "شماره واچر",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "صادرکننده",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "طرف حساب",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "مسیر",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "نام انگلیسی مسافر",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "نام فارسی مسافر",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "نرخ قبلی",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "نوع",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "نوع پرداخت",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "وضعیت کوپن",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "کامپیوتر تغییر دهنده",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "کامپیوتر ثبت کننده",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "کلاس",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "کمیسیون",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "کمیسیون تشویقی",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "گزارش فروش",
                table: "SalesReports",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "-",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "FARE",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "I6",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "KU",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "LP",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "PNR1",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "Sign",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "TAX",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "TOTAL",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "Tax قبلی",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "V0",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "ایرلاین",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "بدهکار",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "بستانکار",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "تاریخ",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "تاریخ برگشت",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "تاریخ دریافت",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "تاریخ رفت",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "تاریخ سند",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "تایید حسابداری",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "تخفیف",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "ترتیب",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "تصویر بلیت",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "توضیحات",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "توضیحات فاکتور",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "جریمه",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "خالص Fare",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "خط بعد",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "درخواست کننده",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "درصد کمیسیون",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "سن",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "ش ک فاکتور",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "شماره بلیط",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "شماره بلیط قبلی",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "شماره سند",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "شماره قرارداد",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "شماره واچر",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "صادرکننده",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "طرف حساب",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "مسیر",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "نام انگلیسی مسافر",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "نام فارسی مسافر",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "نرخ قبلی",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "نوع",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "نوع پرداخت",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "وضعیت کوپن",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "کامپیوتر تغییر دهنده",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "کامپیوتر ثبت کننده",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "کلاس",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "کمیسیون",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "کمیسیون تشویقی",
                table: "SalesReports");

            migrationBuilder.DropColumn(
                name: "گزارش فروش",
                table: "SalesReports");
        }
    }
}
