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
