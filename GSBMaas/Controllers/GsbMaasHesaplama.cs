using GSBMaas.Context;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http; // ISession için gerekli


namespace GSBMaas.Controllers
{


    public class GsbMaasHesaplama : Controller
    {
        AppDbContext db = new AppDbContext();

       

            public IActionResult Index()
        {


            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserAd")) ||
            string.IsNullOrEmpty(HttpContext.Session.GetString("UserSoyad")))
            {
                return RedirectToAction("Giris", "Home"); // Giriş yoksa yönlendir
            }

             // Sadece misafir kullanıcı kontrolü
            if (HttpContext.Session.GetString("MisafirKullanici") == "true")
            {
                return RedirectToAction("Index", "Home");
            }


            var deger = db.Sabits.Find(1);
            var aylar = new Dictionary<string, int>
            {
                { "Ocak", 31 },
                { "Şubat", 28 },
                { "Mart", 31 },
                { "Nisan", 30 },
                { "Mayıs", 31 },
                { "Haziran", 30 },
                { "Temmuz", 31 },
                { "Ağustos", 31 },
                { "Eylül", 30 },
                { "Ekim", 31 },
                { "Kasım", 30 },
                { "Aralık", 31 }
            };

            var iller = new Dictionary<string, int>
            {
                { "ADANA", 1 },
                { "ADIYAMAN", 0 },
                { "AFYONKARAHİSAR", 0 },
                { "AĞRI", 0 },
                { "AMASYA", 0 },
                { "ANKARA", 1 },
                { "ANTALYA", 1 },
                { "ARTVİN", 0 },
                { "AYDIN", 1 },
                { "BALIKESİR", 1 },
                { "BİLECİK", 0 },
                { "BİNGÖL", 0 },
                { "BİTLİS", 0 },
                { "BOLU", 0 },
                { "BURDUR", 0 },
                { "BURSA", 1 },
                { "ÇANAKKALE", 0 },
                { "ÇANKIRI", 0 },
                { "ÇORUM", 0 },
                { "DENİZLİ", 1 },
                { "DİYARBAKIR", 1 },
                { "EDİRNE", 0 },
                { "ELAZIĞ", 0 },
                { "ERZİNCAN", 0 },
                { "ERZURUM", 1 },
                { "ESKİŞEHİR", 1 },
                { "GAZİANTEP", 1 },
                { "GİRESUN", 0 },
                { "GÜMÜŞHANE", 0 },
                { "HAKKARİ", 0 },
                { "HATAY", 1 },
                { "ISPARTA", 0 },
                { "MERSİN", 1 },
                { "İSTANBUL", 1 },
                { "İZMİR", 1 },
                { "KARS", 0 },
                { "KASTAMONU", 0 },
                { "KAYSERİ", 1 },
                { "KIRKLARELİ", 0 },
                { "KIRŞEHİR", 0 },
                { "KOCAELİ", 1 },
                { "KONYA", 1 },
                { "KÜTAHYA", 0 },
                { "MALATYA", 1 },
                { "MANİSA", 1 },
                { "KAHRAMANMARAŞ", 1 },
                { "MARDİN", 1 },
                { "MUĞLA", 1 },
                { "MUŞ", 0 },
                { "NEVŞEHİR", 0 },
                { "NİĞDE", 0 },
                { "ORDU", 1 },
                { "RİZE", 0 },
                { "SAKARYA", 1 },
                { "SAMSUN", 1 },
                { "SİİRT", 0 },
                { "SİNOP", 0 },
                { "SİVAS", 0 },
                { "TEKİRDAĞ", 1 },
                { "TOKAT", 0 },
                { "TRABZON", 1 },
                { "TUNCELİ", 0 },
                { "ŞANLIURFA", 1 },
                { "UŞAK", 0 },
                { "VAN", 1 },
                { "YOZGAT", 0 },
                { "ZONGULDAK", 0 },
                { "AKSARAY", 0 },
                { "BAYBURT", 0 },
                { "KARAMAN", 0 },
                { "KIRIKKALE", 0 },
                { "BATMAN", 0 },
                { "ŞIRNAK", 0 },
                { "BARTIN", 0 },
                { "ARDAHAN", 0 },
                { "IĞDIR", 0 },
                { "YALOVA", 0 },
                { "KARABÜK", 0 },
                { "KİLİS", 0 },
                { "OSMANİYE", 0 },
                { "DÜZCE", 0 }
            };
            
            ViewBag.Yevmiye1 = deger.Yevmiye1;
            ViewBag.Yevmiye2 = deger.Yevmiye2;
            ViewBag.Yevmiye3 = deger.Yevmiye3;

            ViewBag.VergiDilimOran1 = deger.VergiDilimOran1;
            ViewBag.VergiDilimOran2 = deger.VergiDilimOran2;
            ViewBag.VergiDilimOran3 = deger.VergiDilimOran3;

            ViewBag.VergiDilimKatsayi1 = deger.VergiDilimKatsayi1;
            ViewBag.VergiDilimKatsayi2 = deger.VergiDilimKatsayi2;
            ViewBag.VergiDilimKatsayi3 = deger.VergiDilimKatsayi3;

            ViewBag.Iller = iller;
            // Test edilecek tarih (örneğin, Ocak ayı)
            DateTime testDate = new DateTime(2024, 5, 1);

            // Test tarihine göre ayları listeleyin
            //var currentMonth = testDate.Month;
            var currentMonth = DateTime.Now.Month;

            var filteredAylar = aylar.Skip(currentMonth - 2)
                                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);


            ViewBag.Aylar = filteredAylar;
            return View();
        }
        public IActionResult ToplamaIslemi(
            int fiilicalisma,int gececalisma,DateTime kidemyili,int calismasaati,
            string gorev,int yogurtparasisaati, int fazlacalismasaati,string iller,
            int ildurum, int aybilgisi, string aybilgisi2,string yevmiyeler,
            int vergioran,string vergicarpan,int cocuksayisi, int gaziyardim,
            int engelliderecesi,float ubgtgunsayisi
            )
        {
            var deger = db.Sabits.Find(1);
            float yevmiyefloat = float.Parse(yevmiyeler, System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));
            float vergicarpanfloat = float.Parse(vergicarpan, System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));
            DateTime bugun = DateTime.Now;
            
            //KENAN DAHİL OLDU :)
            if (kidemyili.Year < 2018)
            {
                kidemyili = new DateTime(2018, 4, 1);
            }

            TimeSpan timeSpan = bugun - kidemyili;
            int years = timeSpan.Days / 365;
            int remainingDays = timeSpan.Days % 365;
            int months = remainingDays / 30;
            int days = remainingDays % 30;


            double result = (double)years + (double)months / 12 + (double)days / 365;


            int kidemyilisayi = (int)Math.Floor(result);

            float vergiayistisnatutari = 0;

            switch (aybilgisi2)
            {
                case "Ocak":
                    vergiayistisnatutari = deger.OcakVergi;
                    break;
                case "Şubat":
                    vergiayistisnatutari = deger.SubatVergi;
                    break;
                case "Mart":
                    vergiayistisnatutari = deger.MartVergi;
                    break;
                case "Nisan":
                    vergiayistisnatutari = deger.NisanVergi;
                    break;
                case "Mayıs":
                    vergiayistisnatutari = deger.MayisVergi;
                    break;
                case "Haziran":
                    vergiayistisnatutari = deger.HaziranVergi;
                    break;
                case "Temmuz":
                    vergiayistisnatutari = deger.TemmuzVergi;
                    break;
                case "Ağustos":
                    vergiayistisnatutari = deger.AgustosVergi;
                    break;
                case "Eylül":
                    vergiayistisnatutari = deger.EylulVergi;
                    break;
                case "Ekim":
                    vergiayistisnatutari = deger.EkimVergi;
                    break;
                case "Kasım":
                    vergiayistisnatutari = deger.KasimVergi;
                    break;
                case "Aralık":
                    vergiayistisnatutari = deger.AralikVergi;
                    break;
            }
            float engelliDerecesiRakam = 0;

            switch (engelliderecesi)
            {
                case 0:
                    engelliDerecesiRakam = 0;
                    break;
                case 1:
                    engelliDerecesiRakam = deger.EngelliDerece1;
                    break;
                case 2:
                    engelliDerecesiRakam = deger.EngelliDerece2;
                    break;
                case 3:
                    engelliDerecesiRakam = deger.EngelliDerece3;
                    break;
            }

            // Hesaplama Sayfası Başlangıç
            float neteCevirmeCarpani = vergicarpanfloat; // 27
            float brutUcret = yevmiyefloat;
            float buyukSehirYol = deger.BuyukSehirYol;
            float kucukSehirYol = deger.KucukSehirYol;
            float birGunYemek = deger.BirGunYemek;
            float birGunYogurt = deger.BirGunYogurt;
            float kresYardimi = deger.KresYardimi;


            
            var vergiIndirimliAylikYevmiye = (brutUcret * aybilgisi * neteCevirmeCarpani) + vergiayistisnatutari;
            var birlestirilmisSosyalYardim = neteCevirmeCarpani * deger.SosyalYardim;
            var birGunYogurtVergili = deger.BirGunYogurt * neteCevirmeCarpani;
            var yuzde6isPrimi = (brutUcret * 6 / 100) * neteCevirmeCarpani;
            var yuzde5isPrimi = (brutUcret * 5 / 100) * neteCevirmeCarpani;
            var birSaatGeceCalismasi = ((brutUcret / 7.5) * 8 / 100) * neteCevirmeCarpani;
            var geceCalismaGelen = gececalisma;
            var birGunYogurt18 = birGunYogurt * neteCevirmeCarpani;
            var yogurParasiGelen = yogurtparasisaati;
            var kresYardimiGelen = cocuksayisi;
            var gaziTerorSehitYakini = brutUcret * aybilgisi * 0.1 * neteCevirmeCarpani; //aybilgisi == sgkCalismaGunu olmalı hata olabilir.
            var gaziGelen = gaziyardim;
            var birGunUBGTMesaisi = brutUcret * 2 * neteCevirmeCarpani;
            var ubgtGun = ubgtgunsayisi;
            var birSaatFazlaCalismaMesaisi = (brutUcret / (7.5)) * 2 * neteCevirmeCarpani;
            var fazlaCalismaGelen = fazlacalismasaati;
            var engelliRakam = (engelliDerecesiRakam * vergioran) / 100;
            var yuzde6IsPrim1Gun = (brutUcret * 6 / 100) * neteCevirmeCarpani;
            var yuzde5IsPrim1Gun = (brutUcret * 5 / 100) * neteCevirmeCarpani;
            var hizmetZamToplam = neteCevirmeCarpani*kidemyilisayi*deger.BirYilHizmetZammi;
            var ikramiye = brutUcret * 30 * neteCevirmeCarpani;
            var ekOdeme= 5526.51 * neteCevirmeCarpani;


            // KENAN DENEME OCAK AYI VERİLERİ 2025
            if (aybilgisi2 == "Aralık")
            {
                ViewBag.Yevmiye1 = 1378.64;
                ViewBag.Yevmiye2 = 1350.11;
                ViewBag.Yevmiye3 = 1340.61;
                brutUcret = yevmiyefloat;
                vergiIndirimliAylikYevmiye = (brutUcret * aybilgisi * neteCevirmeCarpani) + vergiayistisnatutari;
                birlestirilmisSosyalYardim = neteCevirmeCarpani * 3308.16f;
                buyukSehirYol = 2825.73f;
                kucukSehirYol = 1883.82f;
                birGunYemek = 179.22f;
                kresYardimi = 984;
                birSaatGeceCalismasi = ((brutUcret / 7.5) * 8 / 100) * neteCevirmeCarpani;
                geceCalismaGelen = gececalisma;
                birGunYogurt18 = 43.64f * neteCevirmeCarpani;
                yogurParasiGelen = yogurtparasisaati;
                kresYardimiGelen = cocuksayisi;
                gaziTerorSehitYakini = brutUcret * aybilgisi * 0.1 * neteCevirmeCarpani; //aybilgisi == sgkCalismaGunu olmalı hata olabilir.
                gaziGelen = gaziyardim;
                birGunUBGTMesaisi = brutUcret * 2 * neteCevirmeCarpani;
                ubgtGun = ubgtgunsayisi;
                birSaatFazlaCalismaMesaisi = (brutUcret / (7.5)) * 2 * neteCevirmeCarpani;
                fazlaCalismaGelen = fazlacalismasaati;
                engelliRakam = (engelliDerecesiRakam * vergioran) / 100;
                yuzde6IsPrim1Gun = (brutUcret * 6 / 100) * neteCevirmeCarpani;
                yuzde5IsPrim1Gun = (brutUcret * 5 / 100) * neteCevirmeCarpani;
                hizmetZamToplam = neteCevirmeCarpani * kidemyilisayi * deger.BirYilHizmetZammi;
                ikramiye = brutUcret * 30 * neteCevirmeCarpani;
                ekOdeme = 5526.51 * neteCevirmeCarpani;
            }

            var aidat = brutUcret * (100 - vergioran) / 100;
            // Hesaplama Sayfası Bitiş

            // SONUÇLAR BAŞLANGIÇ

            var temizlikGorevlisiBuyukSehir = vergiIndirimliAylikYevmiye + birlestirilmisSosyalYardim + buyukSehirYol + (fiilicalisma * birGunYemek) + (fiilicalisma * yuzde5IsPrim1Gun) + (birSaatGeceCalismasi * geceCalismaGelen) + (birGunYogurt18 * yogurParasiGelen) + (kresYardimi * kresYardimiGelen) + (gaziTerorSehitYakini * gaziGelen) + (birGunUBGTMesaisi * ubgtGun) + (birSaatFazlaCalismaMesaisi * fazlaCalismaGelen) + engelliRakam + hizmetZamToplam - aidat;
            var temizlikGorevlisiKucukSehir = vergiIndirimliAylikYevmiye + birlestirilmisSosyalYardim + kucukSehirYol + (fiilicalisma * birGunYemek) + (fiilicalisma * yuzde5IsPrim1Gun) + (birSaatGeceCalismasi * geceCalismaGelen) + (birGunYogurt18 * yogurParasiGelen) + (kresYardimi * kresYardimiGelen) + (gaziTerorSehitYakini * gaziGelen) + (birGunUBGTMesaisi * ubgtGun) + (birSaatFazlaCalismaMesaisi * fazlaCalismaGelen) + engelliRakam + hizmetZamToplam - aidat;
            var onIkiSaatGuvenlikGorevlisiBuyukSehir = vergiIndirimliAylikYevmiye + birlestirilmisSosyalYardim + buyukSehirYol + (birGunYemek * 2 * fiilicalisma) + (fiilicalisma * yuzde6IsPrim1Gun) + (birSaatGeceCalismasi * geceCalismaGelen) + (kresYardimi * kresYardimiGelen) + (gaziTerorSehitYakini * gaziGelen) + (birGunUBGTMesaisi * ubgtGun) + (birSaatFazlaCalismaMesaisi * fazlaCalismaGelen) + engelliRakam + hizmetZamToplam - aidat;
            var onIkiSaatGuvenlikGorevlisiKucukSehir = vergiIndirimliAylikYevmiye + birlestirilmisSosyalYardim + kucukSehirYol + (birGunYemek * 2 * fiilicalisma) + (fiilicalisma * yuzde6IsPrim1Gun) + (birSaatGeceCalismasi * geceCalismaGelen) + (kresYardimi * kresYardimiGelen) + (gaziTerorSehitYakini * gaziGelen) + (birGunUBGTMesaisi * ubgtGun) + (birSaatFazlaCalismaMesaisi * fazlaCalismaGelen) + engelliRakam + hizmetZamToplam - aidat;
            var sekizSaatGuvenlikGorevlisiBuyukSehir = vergiIndirimliAylikYevmiye + birlestirilmisSosyalYardim + buyukSehirYol + (birGunYemek * fiilicalisma) + (fiilicalisma * yuzde6IsPrim1Gun) + (birSaatGeceCalismasi * geceCalismaGelen) + (kresYardimi * kresYardimiGelen) + (gaziTerorSehitYakini * gaziGelen) + (birGunUBGTMesaisi * ubgtGun) + (birSaatFazlaCalismaMesaisi * fazlaCalismaGelen) + engelliRakam + hizmetZamToplam - aidat;
            var sekizSaatGuvenlikGorevlisiKucukSehir = vergiIndirimliAylikYevmiye + birlestirilmisSosyalYardim + kucukSehirYol + (birGunYemek * fiilicalisma) + (fiilicalisma * yuzde6IsPrim1Gun) + (birSaatGeceCalismasi * geceCalismaGelen) + (kresYardimi * kresYardimiGelen) + (gaziTerorSehitYakini * gaziGelen) + (birGunUBGTMesaisi * ubgtGun) + (birSaatFazlaCalismaMesaisi * fazlaCalismaGelen) + engelliRakam + hizmetZamToplam - aidat;
            var sekizSaatIklimlendirmeBuyukSehir = vergiIndirimliAylikYevmiye + birlestirilmisSosyalYardim + buyukSehirYol + (birGunYemek * fiilicalisma) + (fiilicalisma * yuzde6IsPrim1Gun) + (birSaatGeceCalismasi * geceCalismaGelen) + (birGunYogurt18 * yogurParasiGelen) + (gaziTerorSehitYakini * gaziGelen) + (birGunUBGTMesaisi * ubgtGun) + (kresYardimi * kresYardimiGelen) + (birSaatFazlaCalismaMesaisi * fazlaCalismaGelen) + engelliRakam + hizmetZamToplam - aidat;
            var sekizSaatIklimlendirmeKucukSehir = vergiIndirimliAylikYevmiye + birlestirilmisSosyalYardim + kucukSehirYol + (birGunYemek * fiilicalisma) + (fiilicalisma * yuzde6IsPrim1Gun) + (birSaatGeceCalismasi * geceCalismaGelen) + (birGunYogurt18 * yogurParasiGelen) + (gaziTerorSehitYakini * gaziGelen) + (birGunUBGTMesaisi * ubgtGun) + (kresYardimi * kresYardimiGelen) + (birSaatFazlaCalismaMesaisi * fazlaCalismaGelen) + engelliRakam + hizmetZamToplam - aidat;
            var bakimOnarimTeknisyenBuyukSehir = vergiIndirimliAylikYevmiye + birlestirilmisSosyalYardim + buyukSehirYol + (fiilicalisma * birGunYemek) + (fiilicalisma * yuzde6IsPrim1Gun) + (birSaatGeceCalismasi * geceCalismaGelen) + (birGunYogurt18 * yogurParasiGelen) + (gaziTerorSehitYakini * gaziGelen) + (birGunUBGTMesaisi * ubgtGun) + (kresYardimi * kresYardimiGelen) + (birSaatFazlaCalismaMesaisi * fazlaCalismaGelen) + engelliRakam + hizmetZamToplam - aidat;
            var bakimOnarimTeknisyenKucukSehir = vergiIndirimliAylikYevmiye + birlestirilmisSosyalYardim + kucukSehirYol + (fiilicalisma * birGunYemek) + (fiilicalisma * yuzde6IsPrim1Gun) + (birSaatGeceCalismasi * geceCalismaGelen) + (birGunYogurt18 * yogurParasiGelen) + (gaziTerorSehitYakini * gaziGelen) + (birGunUBGTMesaisi * ubgtGun) + (kresYardimi * kresYardimiGelen) + (birSaatFazlaCalismaMesaisi * fazlaCalismaGelen) + engelliRakam + hizmetZamToplam - aidat;
            var onIkiSaatBakimOnarimBuyukSehir = vergiIndirimliAylikYevmiye + birlestirilmisSosyalYardim + buyukSehirYol + (birGunYemek * 2 * fiilicalisma) + (fiilicalisma * yuzde6IsPrim1Gun) + (birSaatGeceCalismasi * geceCalismaGelen) + (birGunYogurt18 * yogurParasiGelen) + (kresYardimi * kresYardimiGelen) + (gaziTerorSehitYakini * gaziGelen) + (birGunUBGTMesaisi * ubgtGun) + (birSaatFazlaCalismaMesaisi * fazlaCalismaGelen) + engelliRakam + hizmetZamToplam - aidat;
            var onIkiSaatBakimOnarimKucukSehir = vergiIndirimliAylikYevmiye + birlestirilmisSosyalYardim + kucukSehirYol + (birGunYemek * 2 * fiilicalisma) + (fiilicalisma * yuzde6IsPrim1Gun) + (birSaatGeceCalismasi * geceCalismaGelen) + (birGunYogurt18 * yogurParasiGelen) + (kresYardimi * kresYardimiGelen) + (gaziTerorSehitYakini * gaziGelen) + (birGunUBGTMesaisi * ubgtGun) + (birSaatFazlaCalismaMesaisi * fazlaCalismaGelen) + engelliRakam + hizmetZamToplam - aidat;

            // SONUÇLAR BİTİŞ

            var veri = "Fiili Çalışma: " +fiilicalisma+ 
                " Temizlik Görevlisi Büyük Şehir: " + temizlikGorevlisiBuyukSehir +
                " Temizlik Görevlisi Küçük Şehir: " + temizlikGorevlisiKucukSehir +
                " 12 Saat Güvenlik Görevlisi Büyük Şehir: " + onIkiSaatGuvenlikGorevlisiBuyukSehir +
                " 12 Saat Güvenlik Görevlisi Küçük Şehir: " + onIkiSaatGuvenlikGorevlisiKucukSehir +
                " 8 Saat Güvenlik Görevlisi Büyük Şehir: " + sekizSaatGuvenlikGorevlisiBuyukSehir +
                " 8 Saat Güvenlik Görevlisi Küçük Şehir: " + sekizSaatGuvenlikGorevlisiKucukSehir +
                " 8 Saat İklimledirme Görevlisi Büyük Şehir: " + sekizSaatIklimlendirmeBuyukSehir +
                " 8 Saat İklimledirme Görevlisi Küçük Şehir: " + sekizSaatIklimlendirmeKucukSehir +
                " Teknisyen Büyük Şehir: " + bakimOnarimTeknisyenBuyukSehir +
                " Teknisyen Küçük Şehir: " + bakimOnarimTeknisyenKucukSehir +
                " 12 Saat Bakım Onarım Büyük Şehir: " + onIkiSaatBakimOnarimBuyukSehir +
                " 12 Saat Bakım Onarım Küçük Şehir: " + onIkiSaatBakimOnarimKucukSehir +

                " İller: " + iller +

                " İl Durum: " + ildurum + 
                " Ay Bilgisi1: " + aybilgisi + 
                " Ay Bilgisi2: " + aybilgisi2 + 
                " Gece Çalışma: " + gececalisma + 
                " Yoğurt Parası Saati: " + yogurtparasisaati + 
                " Fazla Çalışma Saati: " + fazlacalismasaati + 
                " Kıdem Yılı: " + kidemyili + 
                " Çalışma Saati: " + calismasaati + 
                " Yevmiye: " + yevmiyefloat.ToString() + 
                " Vergi Oranı: " + vergioran + 
                " Vergi Çarpanı: " + vergicarpanfloat + 
                " Çocuk Sayısı: " + cocuksayisi + 
                " Gazi Yardım: " + gaziyardim + 
                " Engelli Derecesi : " + engelliderecesi + 
                " UBGT Gün Sayısı : " + ubgtgunsayisi + 
                " VERGİ AY İSTİSNA TUTARI : " + vergiIndirimliAylikYevmiye + 
                " Görev: " + gorev;
            
            //var veri = kidemyili;
            var toplam = JsonConvert.SerializeObject(veri).ToString();
            //return Json(toplam);
            //var toplam = JsonConvert.SerializeObject(kidemyili).ToString();


            //if (aybilgisi2 == "Nisan" || aybilgisi2 == "Ağustos")
            //{

            //}
            //else
            //{

            //}
            

            double sonuc = 0;
            float vergiFarki1 = 2560.34f;
            float vergiFarki2 = 2507.36f;
            float vergiFarki3 = 2489.72f;
            if (vergioran == 20) { vergiFarki1 = 2551.77f; vergiFarki2 = 2498.97f; vergiFarki3 = 2481.38f; }
            float tediyeYevmiye1 = 16109.90f;
            float tediyeYevmiye2 = 15776.52f;
            float tediyeYevmiye3 = 15665.59f;


            if (aybilgisi2 == "Kasım" && yevmiyeler == "1378,64")
            {
                if (gorev == "Temizlik" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(temizlikGorevlisiBuyukSehir + tediyeYevmiye1, 2) + " TL dir").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Temizlik" && ildurum == 0)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(temizlikGorevlisiKucukSehir + tediyeYevmiye1, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir + tediyeYevmiye1, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 0)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir + tediyeYevmiye1, 2) + " TL").ToString());
                }

                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir + tediyeYevmiye1, 2) + " TL").ToString());
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir + tediyeYevmiye1, 2) + " TL").ToString());

                }
                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 12)
                {   
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(onIkiSaatBakimOnarimBuyukSehir + tediyeYevmiye1, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(onIkiSaatBakimOnarimKucukSehir + tediyeYevmiye1, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(sekizSaatGuvenlikGorevlisiBuyukSehir + tediyeYevmiye1, 2) + " TL").ToString());
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(sekizSaatGuvenlikGorevlisiKucukSehir + tediyeYevmiye1, 2) + " TL").ToString());

                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiBuyukSehir + tediyeYevmiye1, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiKucukSehir + tediyeYevmiye1, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else
                {
                    return Json(sonuc);
                }
            }
            if (aybilgisi2 == "Kasım" && yevmiyeler == "1350,11")
            {
                if (gorev == "Temizlik" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(temizlikGorevlisiBuyukSehir + tediyeYevmiye2, 2) + " TL dir").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Temizlik" && ildurum == 0)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(temizlikGorevlisiKucukSehir + tediyeYevmiye2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir + tediyeYevmiye2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 0)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir + tediyeYevmiye2, 2) + " TL").ToString());
                }

                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir + tediyeYevmiye2, 2) + " TL").ToString());
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir + tediyeYevmiye2, 2) + " TL").ToString());

                }
                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(onIkiSaatBakimOnarimBuyukSehir + tediyeYevmiye2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(onIkiSaatBakimOnarimKucukSehir + tediyeYevmiye1, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(sekizSaatGuvenlikGorevlisiBuyukSehir + tediyeYevmiye2, 2) + " TL").ToString());
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(sekizSaatGuvenlikGorevlisiKucukSehir + tediyeYevmiye2, 2) + " TL").ToString());

                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiBuyukSehir + tediyeYevmiye2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiKucukSehir + tediyeYevmiye2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else
                {
                    return Json(sonuc);
                }
            }
            if (aybilgisi2 == "Kasım" && yevmiyeler == "1340,61")
            {
                if (gorev == "Temizlik" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(temizlikGorevlisiBuyukSehir + tediyeYevmiye3, 2) + " TL dir").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Temizlik" && ildurum == 0)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(temizlikGorevlisiKucukSehir + tediyeYevmiye3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir + tediyeYevmiye3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 0)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir + tediyeYevmiye3, 2) + " TL").ToString());
                }

                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir + tediyeYevmiye3, 2) + " TL").ToString());
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir + tediyeYevmiye3, 2) + " TL").ToString());

                }
                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(onIkiSaatBakimOnarimBuyukSehir + tediyeYevmiye3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(onIkiSaatBakimOnarimKucukSehir + tediyeYevmiye3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(sekizSaatGuvenlikGorevlisiBuyukSehir + tediyeYevmiye3, 2) + " TL").ToString());
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(sekizSaatGuvenlikGorevlisiKucukSehir + tediyeYevmiye3, 2) + " TL").ToString());

                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiBuyukSehir + tediyeYevmiye3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam (MAAŞ + TEDİYE) " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiKucukSehir + tediyeYevmiye3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else
                {
                    return Json(sonuc);
                }
            }

            // BURAYA YENİ GELİŞME YAPIYORUM MART TEDİYESİ İÇİN KENAN
            // VERGİ İSTİSNASI 1 BAŞLANGIÇ
            if (aybilgisi2 == "Mart" && yevmiyeler == "1105,3")
            {
                if (gorev == "Temizlik" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam :  " + (float)Math.Round(temizlikGorevlisiBuyukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Temizlik" && ildurum == 0)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(temizlikGorevlisiKucukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 0)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString());
                }

                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString());
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString());

                }
                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimBuyukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimKucukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiBuyukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString());
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiKucukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString());

                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiBuyukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiKucukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else
                {
                    return Json(sonuc);
                }
            }
            // VERGİ İSTİSNASI 2 BAŞLANGIÇ

            if (aybilgisi2 == "Mart" && yevmiyeler == "1082,43")
            {
                if (gorev == "Temizlik" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(temizlikGorevlisiBuyukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Temizlik" && ildurum == 0)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(temizlikGorevlisiKucukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 0)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir - vergiFarki2, 2) + " TL").ToString());
                }

                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir - vergiFarki2, 2) + " TL").ToString());
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir - vergiFarki2, 2) + " TL").ToString());

                }
                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimBuyukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimKucukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiBuyukSehir - vergiFarki2, 2) + " TL").ToString());
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiKucukSehir - vergiFarki2, 2) + " TL").ToString());

                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiBuyukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiKucukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else
                {
                    return Json(sonuc);
                }
            }

            // VERGİ İSTİSNASI 3 BAŞLANGIÇ
            if (aybilgisi2 == "Mart" && yevmiyeler == "1074,81")
            {
                if (gorev == "Temizlik" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(temizlikGorevlisiBuyukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Temizlik" && ildurum == 0)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(temizlikGorevlisiKucukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 0)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir - vergiFarki3, 2) + " TL").ToString());
                }

                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir - vergiFarki3, 2) + " TL").ToString());
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir - vergiFarki3, 2) + " TL").ToString());

                }
                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimBuyukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimKucukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiBuyukSehir - vergiFarki3, 2) + " TL").ToString());
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiKucukSehir - vergiFarki3, 2) + " TL").ToString());

                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiBuyukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiKucukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else
                {
                    return Json(sonuc);
                }
            }

            // KENAN MART TEDİYE DENEME SONU


            // BURAYA YENİ BİR DENEME YAPIYORUM KENAN
            // BURAYA YENİ GELİŞME YAPIYORUM OCAK TEDİYESİ İÇİN KENAN
            // VERGİ İSTİSNASI 1 BAŞLANGIÇ
            if (aybilgisi2 == "Ocak" && yevmiyeler == "1457,91")
            {
                if (gorev == "Temizlik" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam :  " + (float)Math.Round(temizlikGorevlisiBuyukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Temizlik" && ildurum == 0)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(temizlikGorevlisiKucukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 0)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString());
                }

                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString());
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString());

                }
                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimBuyukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimKucukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiBuyukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString());
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiKucukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString());

                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiBuyukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiKucukSehir - vergiFarki1, 2) + " TL ." + "............ BİLGİ: Vergi İstisna Tutarının " + vergiFarki1 + " Kadar Kısmı Tediyede Kullanıldığından Dolayı Maaşınız Daha Az Yatmıştır").ToString();
                    return Json(sonuc0);
                }
                else
                {
                    return Json(sonuc);
                }
            }
            // VERGİ İSTİSNASI 2 BAŞLANGIÇ

            if (aybilgisi2 == "Ocak" && yevmiyeler == "1427,74")
            {
                if (gorev == "Temizlik" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(temizlikGorevlisiBuyukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Temizlik" && ildurum == 0)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(temizlikGorevlisiKucukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 0)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir - vergiFarki2, 2) + " TL").ToString());
                }

                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir - vergiFarki2, 2) + " TL").ToString());
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir - vergiFarki2, 2) + " TL").ToString());

                }
                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimBuyukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimKucukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiBuyukSehir - vergiFarki2, 2) + " TL").ToString());
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiKucukSehir - vergiFarki2, 2) + " TL").ToString());

                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiBuyukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiKucukSehir - vergiFarki2, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else
                {
                    return Json(sonuc);
                }
            }

            // VERGİ İSTİSNASI 3 BAŞLANGIÇ
            if (aybilgisi2 == "Ocak" && yevmiyeler == "1417,70")
            {
                if (gorev == "Temizlik" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(temizlikGorevlisiBuyukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Temizlik" && ildurum == 0)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(temizlikGorevlisiKucukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 0)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir - vergiFarki3, 2) + " TL").ToString());
                }

                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir - vergiFarki3, 2) + " TL").ToString());
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir - vergiFarki3, 2) + " TL").ToString());

                }
                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimBuyukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimKucukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiBuyukSehir - vergiFarki3, 2) + " TL").ToString());
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiKucukSehir - vergiFarki3, 2) + " TL").ToString());

                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiBuyukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiKucukSehir - vergiFarki3, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else
                {
                    return Json(sonuc);
                }
            }

            // İKRAMİYE BAŞLANGIÇ
            if (aybilgisi2 == "Ağustos")
            {
                if (gorev == "Temizlik" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam ( MAAŞ + İKRAMİYE ) " + (float)Math.Round(temizlikGorevlisiBuyukSehir + ikramiye, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Temizlik" && ildurum == 0)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam ( MAAŞ + İKRAMİYE ) " + (float)Math.Round(temizlikGorevlisiKucukSehir + ikramiye, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam ( MAAŞ + İKRAMİYE ) " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir + ikramiye, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 0)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam ( MAAŞ + İKRAMİYE ) " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir + ikramiye, 2) + " TL").ToString());
                }

                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam ( MAAŞ + İKRAMİYE ) " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir + ikramiye, 2) + " TL").ToString());
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam ( MAAŞ + İKRAMİYE ) " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir + ikramiye, 2) + " TL").ToString());

                }
                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam ( MAAŞ + İKRAMİYE ) " + (float)Math.Round(onIkiSaatBakimOnarimBuyukSehir + ikramiye, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam ( MAAŞ + İKRAMİYE ) " + (float)Math.Round(onIkiSaatBakimOnarimKucukSehir + ikramiye, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam ( MAAŞ + İKRAMİYE ) " + (float)Math.Round(sekizSaatGuvenlikGorevlisiBuyukSehir + ikramiye, 2) + " TL").ToString());
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam ( MAAŞ + İKRAMİYE ) " + (float)Math.Round(sekizSaatGuvenlikGorevlisiKucukSehir + ikramiye, 2) + " TL").ToString());

                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam ( MAAŞ + İKRAMİYE ) " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiBuyukSehir + ikramiye, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam  ( MAAŞ + İKRAMİYE ) " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiKucukSehir + ikramiye, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else
                {
                    return Json(sonuc);
                }
            }

            // KENAN DENEME SONU


            {
                if (gorev == "Temizlik" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(temizlikGorevlisiBuyukSehir, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Temizlik" && ildurum == 0)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(temizlikGorevlisiKucukSehir, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 1)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Teknisyen" && ildurum == 0)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir, 2) + " TL").ToString());
                }

                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenBuyukSehir, 2) + " TL").ToString());
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(bakimOnarimTeknisyenKucukSehir, 2) + " TL").ToString());

                }
                else if (gorev == "İklimlendirme" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimBuyukSehir, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "İklimlendirme" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatBakimOnarimKucukSehir, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiBuyukSehir, 2) + " TL").ToString());
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 8)
                {
                    return Json(JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(sekizSaatGuvenlikGorevlisiKucukSehir, 2) + " TL").ToString());

                }
                else if (gorev == "Guvenlik" && ildurum == 1 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiBuyukSehir, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else if (gorev == "Guvenlik" && ildurum == 0 && calismasaati == 12)
                {
                    string sonuc0 = JsonConvert.SerializeObject("Yatacak Net Rakam " + (float)Math.Round(onIkiSaatGuvenlikGorevlisiKucukSehir, 2) + " TL").ToString();
                    return Json(sonuc0);
                }
                else
                {
                    return Json(sonuc);
                }
            }

            


            }
    }
}
