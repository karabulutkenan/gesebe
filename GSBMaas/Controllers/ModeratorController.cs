using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using GSBMaas.Models;
using GSBMaas.Context;
using System.Linq;
using BCrypt.Net;
using System;

namespace GSBMaas.Controllers
{
    public class ModeratorController : Controller
    {
        private const string Scheme = "ModeratorAuth"; // Moderator için AuthenticationScheme

        private readonly AppDbContext db = new AppDbContext();

        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Giris(string kullaniciAdi, string password)
        {
            // 🔹 Veritabanından moderatörü bul
            var moderator = db.Moderatorler.FirstOrDefault(m => m.KullaniciAdi == kullaniciAdi);

            if (moderator != null && BCrypt.Net.BCrypt.Verify(password, moderator.SifreHash)) // 🔹 Şifre doğrulama
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, moderator.KullaniciAdi),
                    new Claim("ModeratorAd", moderator.Ad),
                    new Claim("ModeratorSoyad", moderator.Soyad)
                };
                var identity = new ClaimsIdentity(claims, Scheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(Scheme, principal);

                // 🔹 Session’a moderatör bilgilerini kaydet
                HttpContext.Session.SetString("ModeratorAd", moderator.Ad);
                HttpContext.Session.SetString("ModeratorSoyad", moderator.Soyad);

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "Hatalı kullanıcı adı veya şifre!";
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

        //SORU CEVAP BÖLÜMÜ BAŞLADI

        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpGet]
        public IActionResult SorularCevaplar()
        {
            var sorular = db.Sorular.OrderByDescending(s => s.SoruTarihi).ToList();
            return View(sorular);
        }

        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpPost]
        public IActionResult EkleSoru(string kategori, string soruMetni, string soruSahibi)
        {
            if (string.IsNullOrEmpty(kategori) || string.IsNullOrEmpty(soruMetni) || string.IsNullOrEmpty(soruSahibi))
            {
                TempData["ErrorMessage"] = "Tüm alanları doldurmalısınız!";
                return RedirectToAction("SorularCevaplar");
            }

            var yeniSoru = new Soru
            {
                Kategori = kategori,
                SoruMetni = soruMetni,
                SoruSahibi = soruSahibi,
                SoruTarihi = DateTime.Now
            };

            db.Sorular.Add(yeniSoru);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Soru başarıyla eklendi.";
            return RedirectToAction("SorularCevaplar");
        }

        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpPost]
        public IActionResult EkleCevap(int id, string cevapMetni, string kaynak)
        {
            var soru = db.Sorular.Find(id);
            if (soru == null || string.IsNullOrEmpty(cevapMetni))
            {
                TempData["ErrorMessage"] = "Cevap eklenirken hata oluştu!";
                return RedirectToAction("SorularCevaplar");
            }

            soru.CevapMetni = cevapMetni;
            soru.Cevaplayan = HttpContext.Session.GetString("ModeratorAd");
            soru.CevapTarihi = DateTime.Now;
            soru.Kaynak = kaynak;
            soru.OnaylandiMi = false; // Cevap eklendi ama onaylanmadı

            db.SaveChanges();

            TempData["SuccessMessage"] = "Cevap başarıyla eklendi. Onaylanması gerekiyor!";
            return RedirectToAction("SorularCevaplar");
        }

        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpPost]
        public IActionResult OnaylaSoru(int id)
        {
            var soru = db.Sorular.Find(id);
            if (soru != null)
            {
                soru.OnaylandiMi = true;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Soru başarıyla onaylandı!";
            }
            else
            {
                TempData["ErrorMessage"] = "Soru bulunamadı!";
            }
            return RedirectToAction("SorularCevaplar");
        }

        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpPost]
        public IActionResult SilSoru(int id)
        {
            var soru = db.Sorular.Find(id);
            if (soru != null)
            {
                db.Sorular.Remove(soru);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Soru başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Soru bulunamadı!";
            }
            return RedirectToAction("SorularCevaplar");
        }

    }
}
