using System.ComponentModel.DataAnnotations;

namespace GSBMaas.Context
{
    public class Sabit
    {
        [Key]
        public int ID { get; set; }
        public float SosyalYardim { get; set; }
        public float VergiIstisnaTutar { get; set; }
        public float BuyukSehirYol { get; set; }
        public float KucukSehirYol { get; set; }
        public float BirGunYemek { get; set; }
        public float BirGunYogurt { get; set; }
        public float BirYilHizmetZammi { get; set; }
        public float KresYardimi { get; set; }
        public float Yevmiye1 { get; set; }
        public float Yevmiye2 { get; set; }
        public float Yevmiye3 { get; set; }
        public float VergiDilimKatsayi1 { get; set; }
        public float VergiDilimKatsayi2 { get; set; }
        public float VergiDilimKatsayi3 { get; set; }
        public int VergiDilimOran1 { get; set; }
        public int VergiDilimOran2 { get; set; }
        public int VergiDilimOran3 { get; set; }
        public float EngelliDerece1 { get; set; }
        public float EngelliDerece2 { get; set; }
        public float EngelliDerece3 { get; set; }
        public float OcakVergi { get; set; }
        public float SubatVergi { get; set; }
        public float MartVergi { get; set; }
        public float NisanVergi { get; set; }
        public float MayisVergi { get; set; }
        public float HaziranVergi { get; set; }
        public float TemmuzVergi { get; set; }
        public float AgustosVergi { get; set; }
        public float EylulVergi { get; set; }
        public float EkimVergi { get; set; }
        public float KasimVergi { get; set; }
        public float AralikVergi { get; set; }
    }
}
