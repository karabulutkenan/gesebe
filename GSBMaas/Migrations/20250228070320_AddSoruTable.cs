using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GSBMaas.Migrations
{
    public partial class AddSoruTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sorular",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kategori = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SoruMetni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoruSahibi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoruTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CevapMetni = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cevaplayan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CevapTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Kaynak = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnaylandiMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorular", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sorular");
        }
    }
}
