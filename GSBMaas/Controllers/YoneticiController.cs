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
        private const string CorrectPassword = "cu4TQHM3znpQf49m";
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
                // Şifre doğru ise, kullanıcının kimliğini belirleyip kimlik doğrulama çerezini oluştur
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "username"), // Kullanıcı adını buraya ekleyin
                // Diğer rolleri veya kullanıcı bilgilerini de burada ekleyebilirsiniz
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Yetkilendirilmiş sayfaya yönlendir
                return RedirectToAction("Index");
            }
            else
            {
                // Şifre yanlış ise, hata mesajıyla birlikte login sayfasını tekrar göster
                ViewBag.ErrorMessage = "Hatalı şifre! Lütfen tekrar deneyin.";
                return View();
            }
        }
        [HttpGet]
        public IActionResult Cikis()
        {
            // Kullanıcının kimlik doğrulama çerezini kaldırarak çıkış yap
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Çıkış işlemi tamamlandıktan sonra ana sayfaya yönlendir
            return RedirectToAction("Index", "Yonetici");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var deger = db.Sabits.Find(1);
            return View(deger);
        }

        [HttpPost]
        public IActionResult Index(Sabit p)
        {

            var deger = db.Sabits.Find(p.ID);
            deger.SosyalYardim = p.SosyalYardim;
            deger.VergiIstisnaTutar = p.VergiIstisnaTutar;
            deger.BuyukSehirYol = p.BuyukSehirYol;
            deger.KucukSehirYol = p.KucukSehirYol;
            deger.BirGunYemek = p.BirGunYemek;
            deger.BirGunYogurt = p.BirGunYogurt;
            deger.BirYilHizmetZammi = p.BirYilHizmetZammi;
            deger.KresYardimi = p.KresYardimi;
            deger.Yevmiye1 = p.Yevmiye1;
            deger.Yevmiye2 = p.Yevmiye2;
            deger.Yevmiye3 = p.Yevmiye3;
            deger.VergiDilimKatsayi1 = p.VergiDilimKatsayi1;
            deger.VergiDilimKatsayi2 = p.VergiDilimKatsayi2;
            deger.VergiDilimKatsayi3 = p.VergiDilimKatsayi3;
            deger.VergiDilimOran1 = p.VergiDilimOran1;
            deger.VergiDilimOran2 = p.VergiDilimOran2;
            deger.VergiDilimOran3 = p.VergiDilimOran3;
            deger.EngelliDerece1 = p.EngelliDerece1;
            deger.EngelliDerece2 = p.EngelliDerece2;
            deger.EngelliDerece3 = p.EngelliDerece3;

            deger.OcakVergi = p.OcakVergi;
            deger.SubatVergi = p.SubatVergi;
            deger.MartVergi = p.MartVergi;
            deger.NisanVergi = p.NisanVergi;
            deger.MayisVergi = p.MayisVergi;
            deger.HaziranVergi = p.HaziranVergi;
            deger.TemmuzVergi = p.TemmuzVergi;
            deger.AgustosVergi = p.AgustosVergi;
            deger.EylulVergi = p.EylulVergi;
            deger.EkimVergi = p.EkimVergi;
            deger.KasimVergi = p.KasimVergi;
            deger.AralikVergi = p.AralikVergi;
            db.SaveChanges();
            return View(deger);
        }
    }
}
