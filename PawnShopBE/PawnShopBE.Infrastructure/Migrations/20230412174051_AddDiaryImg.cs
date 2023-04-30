using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawnShopBE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDiaryImg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiaryImg",
                columns: table => new
                {
                    DiaryImgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterestDiaryId = table.Column<int>(type: "int", nullable: false),
                    ProofImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryImg", x => x.DiaryImgId);
                    table.ForeignKey(
                        name: "FK_DiaryImg_InterestDiary_InterestDiaryId",
                        column: x => x.InterestDiaryId,
                        principalTable: "InterestDiary",
                        principalColumn: "InterestDiaryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogAsset",
                columns: table => new
                {
                    logAssetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contractAssetId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WareHouseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImportImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExportImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogAsset", x => x.logAssetId);
                    table.ForeignKey(
                        name: "FK_LogAsset_ContractAsset_contractAssetId",
                        column: x => x.contractAssetId,
                        principalTable: "ContractAsset",
                        principalColumn: "ContractAssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiaryImg_InterestDiaryId",
                table: "DiaryImg",
                column: "InterestDiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_LogAsset_contractAssetId",
                table: "LogAsset",
                column: "contractAssetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiaryImg");

            migrationBuilder.DropTable(
                name: "LogAsset");
        }
    }
}
