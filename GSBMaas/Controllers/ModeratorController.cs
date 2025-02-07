using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GSBMaas.Models;
using GSBMaas.Context;
using System.Linq;

namespace GSBMaas.Controllers
{
    public class ModeratorController : Controller
    {
        private const string CorrectPassword = "Ekip2025**"; // Moderatör şifresi
        private const string Scheme = "ModeratorAuth"; // Moderator için AuthenticationScheme

        // ❗ AppDbContext DI ile değil, eski haliyle kullanılıyor!
        private readonly AppDbContext db = new AppDbContext();

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
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "moderator")
                };
                var identity = new ClaimsIdentity(claims, Scheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(Scheme, principal);
                return RedirectToAction("Index");
            }
            else
            {
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
            return View();
        }

        private List<string> GetIller()
        {
            return new List<string>
            {
                "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Aksaray", "Amasya", "Ankara", "Antalya", "Ardahan", "Artvin",
                "Aydın", "Balıkesir", "Bartın", "Batman", "Bayburt", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur",
                "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Düzce", "Edirne", "Elazığ", "Erzincan",
                "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkari", "Hatay", "Iğdır", "Isparta", "İstanbul",
                "İzmir", "Kahramanmaraş", "Karabük", "Karaman", "Kars", "Kastamonu", "Kayseri", "Kırıkkale", "Kırklareli", "Kırşehir",
                "Kilis", "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa", "Mardin", "Mersin", "Muğla", "Muş",
                "Nevşehir", "Niğde", "Ordu", "Osmaniye", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas",
                "Şanlıurfa", "Şırnak", "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Uşak", "Van", "Yalova", "Yozgat", "Zonguldak"
            };
        }

        [HttpGet]
        public IActionResult MisafirhaneEkle()
        {
            ViewBag.Iller = GetIller();
            return PartialView("_MisafirhaneEkle");
        }

        [HttpPost]
        public IActionResult MisafirhaneEkle(Misafirhane misafirhane)
        {
            if (ModelState.IsValid)
            {
                db.Misafirhaneler.Add(misafirhane);
                db.SaveChanges();
                return Json(new { success = true, message = "Misafirhane başarıyla eklendi." });
            }

            return Json(new { success = false, message = "Hata oluştu, lütfen tekrar deneyin!" });
        }
    }
}
