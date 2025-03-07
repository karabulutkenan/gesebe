using Microsoft.EntityFrameworkCore.Migrations;

namespace GSBMaas.Migrations
{
    public partial class SoruSoranUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SoruSoran",
                table: "Sorular",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoruSoran",
                table: "Sorular");
        }
    }
}
