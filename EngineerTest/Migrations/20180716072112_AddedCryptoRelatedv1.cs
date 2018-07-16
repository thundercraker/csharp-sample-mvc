using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EngineerTest.Migrations
{
    public partial class AddedCryptoRelatedv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyChoices",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExchangeChoices",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CryptoTrades",
                columns: table => new
                {
                    RunId = table.Column<Guid>(nullable: false),
                    TradeNum = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<long>(nullable: false),
                    BaseCurrency = table.Column<string>(nullable: true),
                    SubCurrency = table.Column<string>(nullable: true),
                    Exchange = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Volume = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoTrades", x => new { x.RunId, x.TradeNum });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryptoTrades");

            migrationBuilder.DropColumn(
                name: "CurrencyChoices",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ExchangeChoices",
                table: "AspNetUsers");
        }
    }
}
