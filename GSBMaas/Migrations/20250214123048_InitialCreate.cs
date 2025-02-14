using Microsoft.EntityFrameworkCore.Migrations;

namespace GSBMaas.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Misafirhaneler",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ili = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SabitTelefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CepTelefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Misafirhaneler", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sabits",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SosyalYardim = table.Column<float>(type: "real", nullable: false),
                    VergiIstisnaTutar = table.Column<float>(type: "real", nullable: false),
                    BuyukSehirYol = table.Column<float>(type: "real", nullable: false),
                    KucukSehirYol = table.Column<float>(type: "real", nullable: false),
                    BirGunYemek = table.Column<float>(type: "real", nullable: false),
                    BirGunYogurt = table.Column<float>(type: "real", nullable: false),
                    BirYilHizmetZammi = table.Column<float>(type: "real", nullable: false),
                    KresYardimi = table.Column<float>(type: "real", nullable: false),
                    Yevmiye1 = table.Column<float>(type: "real", nullable: false),
                    Yevmiye2 = table.Column<float>(type: "real", nullable: false),
                    Yevmiye3 = table.Column<float>(type: "real", nullable: false),
                    VergiDilimKatsayi1 = table.Column<float>(type: "real", nullable: false),
                    VergiDilimKatsayi2 = table.Column<float>(type: "real", nullable: false),
                    VergiDilimKatsayi3 = table.Column<float>(type: "real", nullable: false),
                    VergiDilimOran1 = table.Column<int>(type: "int", nullable: false),
                    VergiDilimOran2 = table.Column<int>(type: "int", nullable: false),
                    VergiDilimOran3 = table.Column<int>(type: "int", nullable: false),
                    EngelliDerece1 = table.Column<float>(type: "real", nullable: false),
                    EngelliDerece2 = table.Column<float>(type: "real", nullable: false),
                    EngelliDerece3 = table.Column<float>(type: "real", nullable: false),
                    OcakVergi = table.Column<float>(type: "real", nullable: false),
                    SubatVergi = table.Column<float>(type: "real", nullable: false),
                    MartVergi = table.Column<float>(type: "real", nullable: false),
                    NisanVergi = table.Column<float>(type: "real", nullable: false),
                    MayisVergi = table.Column<float>(type: "real", nullable: false),
                    HaziranVergi = table.Column<float>(type: "real", nullable: false),
                    TemmuzVergi = table.Column<float>(type: "real", nullable: false),
                    AgustosVergi = table.Column<float>(type: "real", nullable: false),
                    EylulVergi = table.Column<float>(type: "real", nullable: false),
                    EkimVergi = table.Column<float>(type: "real", nullable: false),
                    KasimVergi = table.Column<float>(type: "real", nullable: false),
                    AralikVergi = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sabits", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Misafirhaneler");

            migrationBuilder.DropTable(
                name: "Sabits");
        }
    }
}
