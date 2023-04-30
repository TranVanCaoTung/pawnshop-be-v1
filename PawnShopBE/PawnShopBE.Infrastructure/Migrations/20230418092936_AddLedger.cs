using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawnShopBE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLedger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Ledger");

            migrationBuilder.DropColumn(
                name: "Fund",
                table: "Ledger");

            migrationBuilder.DropColumn(
                name: "LiquidationMoney",
                table: "Ledger");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Ledger");

            migrationBuilder.RenameColumn(
                name: "RecveivedInterest",
                table: "Ledger",
                newName: "Revenue");

            migrationBuilder.RenameColumn(
                name: "ReceivedPrincipal",
                table: "Ledger",
                newName: "Profit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Revenue",
                table: "Ledger",
                newName: "RecveivedInterest");

            migrationBuilder.RenameColumn(
                name: "Profit",
                table: "Ledger",
                newName: "ReceivedPrincipal");

            migrationBuilder.AddColumn<long>(
                name: "Balance",
                table: "Ledger",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "Fund",
                table: "Ledger",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LiquidationMoney",
                table: "Ledger",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Ledger",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
