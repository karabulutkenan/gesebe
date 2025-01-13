using Microsoft.EntityFrameworkCore.Migrations;

namespace GSBMaas.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Sabits");
        }
    }
}
