using Microsoft.EntityFrameworkCore.Migrations;

namespace GSBMaas.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "Moderatorler",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KullaniciAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SifreHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moderatorler", x => x.ID);
                });

           
               
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Misafirhaneler");

            migrationBuilder.DropTable(
                name: "Moderatorler");

            migrationBuilder.DropTable(
                name: "Sabits");
        }
    }
}
