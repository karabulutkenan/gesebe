using GSBMaas.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GSBMaas.Controllers
{
    public class YoneticiController : Controller
    {
        AppDbContext db = new AppDbContext();
        private const string CorrectPassword = "cu4TQHM3znpQf49m"; // Yönetici şifresi
        private const string Scheme = "YoneticiAuth"; // Yonetici için özel AuthenticationScheme

        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Giris(string password)
        {
            if (password == CorrectPassword)
            {
                // Şifre doğru, kimlik doğrulama oluştur
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "yonetici")
                };
                var identity = new ClaimsIdentity(claims, Scheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(Scheme, principal);

                // Yönetici Index sayfasına yönlendir
                return RedirectToAction("Index");
            }
            else
            {
                // Hatalı şifre için hata mesajı göster
                ViewBag.ErrorMessage = "Hatalı şifre! Lütfen tekrar deneyin.";
                return View();
            }
        }
        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpPost]
        public IActionResult Index(Sabit model)
        {
            if (ModelState.IsValid)
            {
                var sabit = db.Sabits.Find(model.ID);
                if (sabit != null)
                {
                    sabit.SosyalYardim = model.SosyalYardim;
                    sabit.VergiIstisnaTutar = model.VergiIstisnaTutar;
                    sabit.BuyukSehirYol = model.BuyukSehirYol;
                    sabit.KucukSehirYol = model.KucukSehirYol;
                    sabit.BirGunYemek = model.BirGunYemek;
                    sabit.BirGunYogurt = model.BirGunYogurt;
                    sabit.BirYilHizmetZammi = model.BirYilHizmetZammi;
                    sabit.KresYardimi = model.KresYardimi;
                    sabit.Yevmiye1 = model.Yevmiye1;
                    sabit.Yevmiye2 = model.Yevmiye2;
                    sabit.Yevmiye3 = model.Yevmiye3;
                    sabit.VergiDilimKatsayi1 = model.VergiDilimKatsayi1;
                    sabit.VergiDilimKatsayi2 = model.VergiDilimKatsayi2;
                    sabit.VergiDilimKatsayi3 = model.VergiDilimKatsayi3;
                    sabit.VergiDilimOran1 = model.VergiDilimOran1;
                    sabit.VergiDilimOran2 = model.VergiDilimOran2;
                    sabit.VergiDilimOran3 = model.VergiDilimOran3;
                    sabit.EngelliDerece1 = model.EngelliDerece1;
                    sabit.EngelliDerece2 = model.EngelliDerece2;
                    sabit.EngelliDerece3 = model.EngelliDerece3;
                    sabit.OcakVergi = model.OcakVergi;
                    sabit.SubatVergi = model.SubatVergi;
                    sabit.MartVergi = model.MartVergi;
                    sabit.NisanVergi = model.NisanVergi;
                    sabit.MayisVergi = model.MayisVergi;
                    sabit.HaziranVergi = model.HaziranVergi;
                    sabit.TemmuzVergi = model.TemmuzVergi;
                    sabit.AgustosVergi = model.AgustosVergi;
                    sabit.EylulVergi = model.EylulVergi;
                    sabit.EkimVergi = model.EkimVergi;
                    sabit.KasimVergi = model.KasimVergi;
                    sabit.AralikVergi = model.AralikVergi;

                    db.SaveChanges(); // **Değişiklikleri kaydet**
                }
            }

            return RedirectToAction("Index"); // Güncelleme sonrası sayfayı yeniden yükle
        }

        [HttpGet]
        public IActionResult Cikis()
        {
            HttpContext.SignOutAsync(Scheme);
            return RedirectToAction("Giris");
        }

        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpGet]
        public IActionResult Index()
        {
            var deger = db.Sabits.Find(1);
            return View(deger);
        }
    }
}
