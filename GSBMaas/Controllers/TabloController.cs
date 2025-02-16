using GSBMaas.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;

namespace GSBMaas.Controllers
{
    public class TabloController : Controller
    {
        AppDbContext db = new AppDbContext();
        public IActionResult Index()
        {
            var deger = db.Sabits.Find(1);

            DateTime bugun = DateTime.Today;
            //int mevcutAyGunSayisi = DateTime.DaysInMonth(bugun.Year, bugun.Month);

            DateTime oncekiAy = bugun.AddMonths(-1);
            int oncekiAyGunSayisi = DateTime.DaysInMonth(oncekiAy.Year, oncekiAy.Month);


            // Ay Adı başlanıç
            CultureInfo trCulture = new CultureInfo("tr-TR");
            string ayAdi = bugun.ToString("MMMM", trCulture);
            // Ay Adı bitiş

            // Hesaplama Sayfası Başlangıç
            float neteCevirmeCarpani = deger.VergiDilimKatsayi1; // 15
            float brutUcret = deger.Yevmiye1;
            float brutUcretOcak = 1378.64f;
            float buyukSehirYol = deger.BuyukSehirYol;
            float buyukSehirYolOcak = 2825.73f;
            float kucukSehirYol = deger.KucukSehirYol;
            float kucukSehirYolOcak = 1883.82f;
            float birGunYemek = deger.BirGunYemek;
            float birGunYemekOcak = 179.22f;

            //var vergiIndrimliYevmiye = (brutUcret * mevcutAyGunSayisi * neteCevirmeCarpani) +deger.VergiIstisnaTutar;
            var vergiIndrimliYevmiye = (brutUcret * oncekiAyGunSayisi * neteCevirmeCarpani) + deger.VergiIstisnaTutar;
            var vergiIndrimliYevmiyeOcak = (brutUcretOcak * oncekiAyGunSayisi * neteCevirmeCarpani) + deger.VergiIstisnaTutar;
            var birlestirilmisSosyalYardim = neteCevirmeCarpani * deger.SosyalYardim;
            var birlestirilmisSosyalYardimOcak = neteCevirmeCarpani * 3308.16;
            var birGunYogurtVergili = deger.BirGunYogurt * neteCevirmeCarpani;
            var birGunYogurtVergiliOcak = 43.64 * neteCevirmeCarpani;
            var yuzde6isPrimi = (brutUcret * 6 / 100) * neteCevirmeCarpani; 
            var yuzde5isPrimi = (brutUcret * 5 / 100) * neteCevirmeCarpani; 
            var birSaatGeceCalismasi = ((brutUcret / 7.5) * 8 / 100) * neteCevirmeCarpani; 
            var besYilHizmetZammiVergili = deger.BirYilHizmetZammi * neteCevirmeCarpani * 6;
            var aidat = brutUcret * (100 - deger.VergiDilimOran1) / 100;
            var yuzde6isPrimiOcak = (brutUcretOcak * 6 / 100) * neteCevirmeCarpani;
            var yuzde5isPrimiOcak = (brutUcretOcak * 5 / 100) * neteCevirmeCarpani;
            var birSaatGeceCalismasiOcak = ((brutUcretOcak / 7.5) * 8 / 100) * neteCevirmeCarpani;
            var besYilHizmetZammiVergiliOcak = deger.BirYilHizmetZammi * neteCevirmeCarpani * 6;
            var aidatOcak = brutUcretOcak * (100 - deger.VergiDilimOran1) / 100;

            var ikramiye = brutUcret * 30 * neteCevirmeCarpani;
            // Hesaplama Sayfası Bitiş

            // Tüm Hesap Başlangıç
            var buyukSehirSabit = (vergiIndrimliYevmiye + birlestirilmisSosyalYardim + buyukSehirYol);
            var kucukSehirSabit = (vergiIndrimliYevmiye + birlestirilmisSosyalYardim + kucukSehirYol);
            var buyukSehirSabitOcak = (vergiIndrimliYevmiyeOcak + birlestirilmisSosyalYardimOcak + buyukSehirYolOcak);
            var kucukSehirSabitOcak = (vergiIndrimliYevmiyeOcak + birlestirilmisSosyalYardimOcak + kucukSehirYolOcak);
            var vergiFarki = 2560.34;
            var tediye = 8806.84f;
            //14gün fark hesaplama başlangıç
            var yevmiyeFark = 2345.46;
            var sosyalYardimFark= 172.04;
            var buyuksehirYolFark = 261.45;
            var kucukSehirYolFark = 174.3;
            var birGunYemekFark = 35.53;
            var birGunYogurtFark = 5.3;
            var giyimYardimiFark = 15.62;
            var birGun5primFark = 8.37;
            var birGun6primFark = 10.05;
            var birSaatGeceFark = 1.79;
            var aidatFark = 93.1;
            var buyukSehirFarkSabit = buyuksehirYolFark + yevmiyeFark + sosyalYardimFark + giyimYardimiFark - aidatFark;
            var kucukSehirFarkSabit = kucukSehirYolFark + yevmiyeFark + sosyalYardimFark + giyimYardimiFark - aidatFark;

             if (bugun.Day >= 17) { ayAdi = "Şubat"; }


            if (ayAdi == "Nisan" || ayAdi == "Eylül")
            {
                ViewBag.Ikramiye = "("+ ayAdi + " Ayı İkramiye Dahil)";
                ViewBag.TemizlikBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde5isPrimi * 26) + besYilHizmetZammiVergili - aidat + ikramiye).ToString("0.00");
                ViewBag.TemizlikKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde5isPrimi * 26) + besYilHizmetZammiVergili - aidat + ikramiye).ToString("0.00");
                ViewBag.OnIkiGuvenlikBuyuk = ((buyukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birSaatGeceCalismasi * 63) + besYilHizmetZammiVergili - aidat + ikramiye).ToString("0.00");
                ViewBag.OnIkiGuvenlikKucuk = ((kucukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birSaatGeceCalismasi * 63) + besYilHizmetZammiVergili - aidat + ikramiye).ToString("0.00");
                ViewBag.SekizGuvenlikBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birSaatGeceCalismasi * 85) - aidat + besYilHizmetZammiVergili + ikramiye).ToString("0.00");
                ViewBag.SekizGuvenlikKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birSaatGeceCalismasi * 85) - aidat + besYilHizmetZammiVergili + ikramiye).ToString("0.00");
                ViewBag.IklimlendirmeBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birGunYogurtVergili * 26) + besYilHizmetZammiVergili + ikramiye - aidat).ToString("0.00");
                ViewBag.IklimlendirmeKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birGunYogurtVergili * 26) + besYilHizmetZammiVergili + ikramiye - aidat).ToString("0.00");
                ViewBag.IklimlendirmeBuyuk12 = ((buyukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birGunYogurtVergili * 15) + (birSaatGeceCalismasi * 63) + besYilHizmetZammiVergili + ikramiye - aidat).ToString("0.00");
                ViewBag.IklimlendirmeKucuk12 = ((kucukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birGunYogurtVergili * 15) + (birSaatGeceCalismasi * 63) + besYilHizmetZammiVergili + ikramiye - aidat).ToString("0.00");
                ViewBag.TeknisyenBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + besYilHizmetZammiVergili - aidat + ikramiye).ToString("0.00");
                ViewBag.TeknisyenKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + besYilHizmetZammiVergili - aidat + ikramiye).ToString("0.00");
            }
            else if (ayAdi == "Haziran")
            {
                ViewBag.Ikramiye = "(" + ayAdi + " Ayı Tediye Dahil)";
                ViewBag.TemizlikBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde5isPrimi * 26) + besYilHizmetZammiVergili - aidat + tediye).ToString("0.00");
                ViewBag.TemizlikKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde5isPrimi * 26) + besYilHizmetZammiVergili - aidat + tediye).ToString("0.00");
                ViewBag.OnIkiGuvenlikBuyuk = ((buyukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birSaatGeceCalismasi * 63) + besYilHizmetZammiVergili - aidat + tediye).ToString("0.00");
                ViewBag.OnIkiGuvenlikKucuk = ((kucukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birSaatGeceCalismasi * 63) + besYilHizmetZammiVergili - aidat + tediye).ToString("0.00");
                ViewBag.SekizGuvenlikBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birSaatGeceCalismasi * 85) - aidat + besYilHizmetZammiVergili + tediye).ToString("0.00");
                ViewBag.SekizGuvenlikKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birSaatGeceCalismasi * 85) - aidat + besYilHizmetZammiVergili + tediye).ToString("0.00");
                ViewBag.IklimlendirmeBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birGunYogurtVergili * 26) + besYilHizmetZammiVergili + tediye - aidat).ToString("0.00");
                ViewBag.IklimlendirmeKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birGunYogurtVergili * 26) + besYilHizmetZammiVergili + tediye - aidat).ToString("0.00");
                ViewBag.IklimlendirmeBuyuk12 = ((buyukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birGunYogurtVergili * 15) + (birSaatGeceCalismasi * 63) + besYilHizmetZammiVergili + tediye - aidat).ToString("0.00");
                ViewBag.IklimlendirmeKucuk12 = ((kucukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birGunYogurtVergili * 15) + (birSaatGeceCalismasi * 63) + besYilHizmetZammiVergili + tediye - aidat).ToString("0.00");

                ViewBag.TeknisyenBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + besYilHizmetZammiVergili - aidat + tediye).ToString("0.00");
                ViewBag.TeknisyenKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + besYilHizmetZammiVergili - aidat + tediye).ToString("0.00");
            }
            else if (ayAdi == "Şubat" )
            {
                ViewBag.Ikramiye = "";
                ViewBag.TemizlikBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde5isPrimi * 26) + besYilHizmetZammiVergili - aidat - vergiFarki).ToString("0.00");
                ViewBag.TemizlikKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde5isPrimi * 26) + besYilHizmetZammiVergili - aidat - vergiFarki).ToString("0.00");
                ViewBag.OnIkiGuvenlikBuyuk = ((buyukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birSaatGeceCalismasi * 72) + besYilHizmetZammiVergili - aidat - vergiFarki).ToString("0.00");
                ViewBag.OnIkiGuvenlikKucuk = ((kucukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birSaatGeceCalismasi * 72) + besYilHizmetZammiVergili - aidat - vergiFarki).ToString("0.00");
                ViewBag.SekizGuvenlikBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birSaatGeceCalismasi * 72) - aidat - vergiFarki + besYilHizmetZammiVergili).ToString("0.00");
                ViewBag.SekizGuvenlikKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birSaatGeceCalismasi * 72) - aidat - vergiFarki + besYilHizmetZammiVergili).ToString("0.00");
                ViewBag.IklimlendirmeBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birGunYogurtVergili * 26) + besYilHizmetZammiVergili - vergiFarki - aidat).ToString("0.00");
                ViewBag.IklimlendirmeKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birGunYogurtVergili * 26) + besYilHizmetZammiVergili - vergiFarki - aidat).ToString("0.00");
                ViewBag.IklimlendirmeBuyuk12 = ((buyukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birGunYogurtVergili * 15) + (birSaatGeceCalismasi * 63) + besYilHizmetZammiVergili  - aidat - vergiFarki).ToString("0.00");
                ViewBag.IklimlendirmeKucuk12 = ((kucukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birGunYogurtVergili * 15) + (birSaatGeceCalismasi * 63) + besYilHizmetZammiVergili - aidat - vergiFarki).ToString("0.00");

                ViewBag.TeknisyenBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + besYilHizmetZammiVergili - aidat - vergiFarki).ToString("0.00");
                ViewBag.TeknisyenKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + besYilHizmetZammiVergili - aidat - vergiFarki).ToString("0.00");
            }

            else if (ayAdi == "Ocak")
            {
                
                ViewBag.Ikramiye = "";
                ViewBag.TemizlikBuyuk = ((buyukSehirSabitOcak) + (birGunYemekOcak * 26) + (yuzde5isPrimiOcak * 26) + besYilHizmetZammiVergiliOcak - aidatOcak).ToString("0.00");
                ViewBag.TemizlikKucuk = ((kucukSehirSabitOcak) + (birGunYemekOcak * 26) + (yuzde5isPrimiOcak * 26) + besYilHizmetZammiVergiliOcak - aidatOcak).ToString("0.00");
                ViewBag.OnIkiGuvenlikBuyuk = ((buyukSehirSabitOcak) + (birGunYemekOcak * 30) + (yuzde6isPrimiOcak * 15) + (birSaatGeceCalismasiOcak * 72) + besYilHizmetZammiVergiliOcak - aidatOcak).ToString("0.00");
                ViewBag.OnIkiGuvenlikKucuk = ((kucukSehirSabitOcak) + (birGunYemekOcak * 30) + (yuzde6isPrimiOcak * 15) + (birSaatGeceCalismasiOcak * 72) + besYilHizmetZammiVergiliOcak - aidatOcak).ToString("0.00");
                ViewBag.SekizGuvenlikBuyuk = ((buyukSehirSabitOcak) + (birGunYemekOcak * 26) + (yuzde6isPrimiOcak * 26) + (birSaatGeceCalismasiOcak * 72) - aidatOcak + besYilHizmetZammiVergiliOcak).ToString("0.00");
                ViewBag.SekizGuvenlikKucuk = ((kucukSehirSabitOcak) + (birGunYemekOcak * 26) + (yuzde6isPrimiOcak * 26) + (birSaatGeceCalismasiOcak * 72) - aidatOcak + besYilHizmetZammiVergiliOcak).ToString("0.00");
                ViewBag.IklimlendirmeBuyuk = ((buyukSehirSabitOcak) + (birGunYemekOcak * 26) + (yuzde6isPrimiOcak * 26) + (birGunYogurtVergiliOcak * 26) + (birSaatGeceCalismasiOcak * 72) + besYilHizmetZammiVergiliOcak - aidatOcak).ToString("0.00");
                ViewBag.IklimlendirmeKucuk = ((kucukSehirSabitOcak) + (birGunYemekOcak * 26) + (yuzde6isPrimiOcak * 26) + (birGunYogurtVergiliOcak * 26) + (birSaatGeceCalismasiOcak * 72) + besYilHizmetZammiVergiliOcak - aidatOcak).ToString("0.00");
                ViewBag.IklimlendirmeBuyuk12 = ((buyukSehirSabitOcak) + (birGunYemekOcak * 30) + (yuzde6isPrimiOcak * 15) + (birGunYogurtVergiliOcak * 15) + (birSaatGeceCalismasiOcak * 72) + besYilHizmetZammiVergiliOcak - aidatOcak).ToString("0.00");
                ViewBag.IklimlendirmeKucuk12 = ((kucukSehirSabitOcak) + (birGunYemekOcak * 30) + (yuzde6isPrimiOcak * 15) + (birGunYogurtVergiliOcak * 15) + (birSaatGeceCalismasiOcak * 72) + besYilHizmetZammiVergiliOcak - aidatOcak).ToString("0.00");
                ViewBag.TeknisyenBuyuk = ((buyukSehirSabitOcak) + (birGunYemekOcak * 26) + (yuzde6isPrimiOcak * 26) + besYilHizmetZammiVergiliOcak - aidatOcak).ToString("0.00");
                ViewBag.TeknisyenKucuk = ((kucukSehirSabitOcak) + (birGunYemekOcak * 26) + (yuzde6isPrimiOcak * 26) + besYilHizmetZammiVergiliOcak - aidatOcak).ToString("0.00");
                
            }



            else
            {
                ViewBag.Ikramiye = "";
                ViewBag.TemizlikBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde5isPrimi * 26) + besYilHizmetZammiVergili - aidat).ToString("0.00");
                ViewBag.TemizlikKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde5isPrimi * 26) + besYilHizmetZammiVergili - aidat).ToString("0.00");
                ViewBag.OnIkiGuvenlikBuyuk = ((buyukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birSaatGeceCalismasi * 72) + besYilHizmetZammiVergili - aidat).ToString("0.00");
                ViewBag.OnIkiGuvenlikKucuk = ((kucukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birSaatGeceCalismasi * 72) + besYilHizmetZammiVergili - aidat).ToString("0.00");
                ViewBag.SekizGuvenlikBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birSaatGeceCalismasi * 72) - aidat + besYilHizmetZammiVergili).ToString("0.00");
                ViewBag.SekizGuvenlikKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birSaatGeceCalismasi * 72) - aidat + besYilHizmetZammiVergili).ToString("0.00");
                ViewBag.IklimlendirmeBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birGunYogurtVergili * 26) + (birSaatGeceCalismasi * 72) + besYilHizmetZammiVergili  - aidat).ToString("0.00");
                ViewBag.IklimlendirmeKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + (birGunYogurtVergili * 26) + (birSaatGeceCalismasi * 72) + besYilHizmetZammiVergili  - aidat).ToString("0.00");
                ViewBag.IklimlendirmeBuyuk12 = ((buyukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birGunYogurtVergili * 15) + (birSaatGeceCalismasi * 72) + besYilHizmetZammiVergili  - aidat).ToString("0.00");
                ViewBag.IklimlendirmeKucuk12 = ((kucukSehirSabit) + (birGunYemek * 30) + (yuzde6isPrimi * 15) + (birGunYogurtVergili * 15) + (birSaatGeceCalismasi * 72) + besYilHizmetZammiVergili - aidat).ToString("0.00");

                ViewBag.TeknisyenBuyuk = ((buyukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + besYilHizmetZammiVergili - aidat).ToString("0.00");
                ViewBag.TeknisyenKucuk = ((kucukSehirSabit) + (birGunYemek * 26) + (yuzde6isPrimi * 26) + besYilHizmetZammiVergili - aidat).ToString("0.00");
            }

            // Tüm Hesap Bitiş

            // 14 GÜN FARK BAŞLAMA

            ViewBag.FarkTemizlikBuyuk = ((buyukSehirFarkSabit) + (birGunYemekFark * 12) + (birGun5primFark * 12)).ToString("0.00");
            ViewBag.FarkTemizlikKucuk = ((kucukSehirFarkSabit) + (birGunYemekFark * 12) + (birGun5primFark * 12)).ToString("0.00");
            ViewBag.FarkOnIkiGuvenlikBuyuk = ((buyukSehirFarkSabit) + (birGunYemekFark * 14) + (birGun6primFark * 7) + (birSaatGeceFark * 27) ).ToString("0.00");
            ViewBag.FarkOnIkiGuvenlikKucuk = ((kucukSehirFarkSabit) + (birGunYemekFark * 14) + (birGun6primFark * 7) + (birSaatGeceFark * 27) ).ToString("0.00");
            ViewBag.FarkSekizGuvenlikBuyuk = ((buyukSehirFarkSabit) + (birGunYemekFark * 12) + (birGun6primFark * 12) + (birSaatGeceFark * 27) ).ToString("0.00");
            ViewBag.FarkSekizGuvenlikKucuk = ((kucukSehirFarkSabit) + (birGunYemekFark * 12) + (birGun6primFark * 12) + (birSaatGeceFark * 27) ).ToString("0.00");
            ViewBag.FarkIklimlendirmeBuyuk = ((buyukSehirFarkSabit) + (birGunYemekFark * 12) + (birGun6primFark * 12) + (birGunYogurtFark * 12) + (birSaatGeceFark * 27)).ToString("0.00");
            ViewBag.FarkIklimlendirmeKucuk = ((kucukSehirFarkSabit) + (birGunYemekFark * 12) + (birGun6primFark * 12) + (birGunYogurtFark * 12) + (birSaatGeceFark * 27)).ToString("0.00");
            ViewBag.FarkIklimlendirmeBuyuk12 = ((buyukSehirFarkSabit) + (birGunYemekFark * 14) + (birGun6primFark * 7) + (birGunYogurtFark * 7) + (birSaatGeceFark * 27) ).ToString("0.00");
            ViewBag.FarkIklimlendirmeKucuk12 = ((kucukSehirFarkSabit) + (birGunYemekFark * 14) + (birGun6primFark * 7) + (birGunYogurtFark * 7) + (birSaatGeceFark * 27) ).ToString("0.00");
            ViewBag.FarkTeknisyenBuyuk = ((buyukSehirFarkSabit) + (birGunYemekFark * 12) + (birGun6primFark * 12)).ToString("0.00");
            ViewBag.FarkTeknisyenKucuk = ((kucukSehirFarkSabit) + (birGunYemekFark * 12) + (birGun6primFark * 12)).ToString("0.00");


            ViewBag.AyAdi = ayAdi;
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserAd")) ||
                string.IsNullOrEmpty(HttpContext.Session.GetString("UserSoyad")))
            {
                return RedirectToAction("Giris", "Home");
            }
            return View();
        }
    }
}
