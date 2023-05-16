using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawnShopBE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNoti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Notification",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "LogAsset",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "LogAsset");
        }
    }
}
