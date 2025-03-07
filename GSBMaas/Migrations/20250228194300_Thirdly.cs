using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GSBMaas.Migrations
{
    public partial class Thirdly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           


            migrationBuilder.CreateTable(
                name: "Sorular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KategoriId = table.Column<int>(type: "int", nullable: false),
                    SoruMetni = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CevapMetni = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Cevaplayan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kaynak = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SoruTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CevapTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OnaylandiMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sorular_SoruKategoriler_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "SoruKategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_KategoriId",
                table: "Sorular",
                column: "KategoriId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Misafirhaneler");

            migrationBuilder.DropTable(
                name: "Moderatorler");

            migrationBuilder.DropTable(
                name: "Sabits");

            migrationBuilder.DropTable(
                name: "Sorular");

            migrationBuilder.DropTable(
                name: "SoruKategoriler");
        }
    }
}
