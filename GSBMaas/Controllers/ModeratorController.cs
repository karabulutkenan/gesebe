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
using Microsoft.EntityFrameworkCore;

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
        [HttpPost]
        public IActionResult SoruKategoriEkle(string Ad)
        {
            try
            {
                if (string.IsNullOrEmpty(Ad))
                {
                    return Json(new { success = false, message = "⚠️ Kategori adı boş olamaz!" });
                }

                var yeniKategori = new SoruKategori { Ad = Ad };
                db.SoruKategoriler.Add(yeniKategori);
                db.SaveChanges();

                return Json(new { success = true, message = "✅ Kategori başarıyla eklendi!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "⚠️ Hata oluştu: " + ex.Message });
            }
        }

        // ✅ Kategori Güncelleme
        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpPost]
        public IActionResult SoruKategoriGuncelle(int id, string Ad)
        {
            try
            {
                var kategori = db.SoruKategoriler.Find(id);
                if (kategori == null)
                {
                    return Json(new { success = false, message = "❌ Kategori bulunamadı!" });
                }

                if (string.IsNullOrEmpty(Ad))
                {
                    return Json(new { success = false, message = "⚠️ Kategori adı boş olamaz!" });
                }

                kategori.Ad = Ad;
                db.SaveChanges();
                return Json(new { success = true, message = "✅ Kategori başarıyla güncellendi!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "⚠️ Hata oluştu: " + ex.Message });
            }
        }


        // ✅ Kategori Silme
        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpDelete]
        public IActionResult SoruKategoriSil(int id)
        {
            try
            {
                var kategori = db.SoruKategoriler.Find(id);
                if (kategori == null)
                {
                    return Json(new { success = false, message = "❌ Kategori bulunamadı!" });
                }

                db.SoruKategoriler.Remove(kategori);
                db.SaveChanges();
                return Json(new { success = true, message = "✅ Kategori başarıyla silindi!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "⚠️ Hata oluştu: " + ex.Message });
            }
        }




        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpGet]
        public IActionResult SorularCevaplar()
        {
            var sorular = db.Sorular.Include(s => s.SoruKategori).ToList();
            var kategoriler = db.SoruKategoriler.ToList();

            ViewBag.SoruKategoriler = kategoriler;
            return View(sorular);
        }

        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpPost]
        public IActionResult SoruEkle(int kategoriId, string SoruMetni, string CevapMetni, string Kaynak)
        {
            try
            {
                if (kategoriId <= 0 || string.IsNullOrEmpty(SoruMetni) || string.IsNullOrEmpty(CevapMetni))
                {
                    return Json(new { success = false, message = "Tüm alanları doldurun!" });
                }

                DateTime now = DateTime.Now;

                var yeniSoru = new Soru
                {
                    KategoriId = kategoriId,
                    SoruMetni = SoruMetni,
                    CevapMetni = CevapMetni,
                    Cevaplayan = "Sistem Yöneticisi",
                    Kaynak = Kaynak,
                    SoruTarihi = now,
                    CevapTarihi = now,
                    OnaylandiMi = true
                };

                db.Sorular.Add(yeniSoru);
                db.SaveChanges();

                return Json(new { success = true, message = "✅ Soru başarıyla eklendi ve onaylandı!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "⚠️ Hata oluştu: " + ex.Message });
            }
        }

        //SORU DÜZENLEME

        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpGet]
        public IActionResult GetSoru(int id)
        {
            var soru = db.Sorular.Find(id);
            if (soru != null)
            {
                return Json(new
                {
                    success = true,
                    kategoriId = soru.KategoriId,
                    soruMetni = soru.SoruMetni,
                    cevapMetni = soru.CevapMetni,
                    kaynak = soru.Kaynak
                });
            }
            return Json(new { success = false, message = "❌ Soru bulunamadı!" });
        }


        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpPost]
        public IActionResult SoruGuncelle(int id, int kategoriId, string SoruMetni, string CevapMetni, string Kaynak)
        {
            try
            {
                var soru = db.Sorular.Find(id);
                if (soru != null)
                {
                    soru.KategoriId = kategoriId;
                    soru.SoruMetni = SoruMetni;
                    soru.CevapMetni = CevapMetni;
                    soru.Kaynak = Kaynak;
                    soru.CevapTarihi = DateTime.Now; // Güncellendiğinde tarihi güncelle

                    db.SaveChanges();
                    return Json(new { success = true, message = "✅ Soru başarıyla güncellendi!" });
                }
                return Json(new { success = false, message = "❌ Soru bulunamadı!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "⚠️ Hata oluştu: " + ex.Message });
            }
        }




        // ✅ Moderatör soruyu onaylayacak
        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpPost]
        public IActionResult SoruOnayla(int id)
        {
            try
            {
                var soru = db.Sorular.Find(id);
                if (soru == null)
                {
                    return Json(new { success = false, message = "❌ Soru bulunamadı!" });
                }

                soru.OnaylandiMi = true;
                soru.CevapTarihi = DateTime.Now;
                db.SaveChanges();

                return Json(new { success = true, message = "✅ Soru başarıyla onaylandı!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "⚠️ Hata oluştu: " + ex.Message });
            }
        }


        // ✅ Moderatör soruyu silebilir
        [Authorize(AuthenticationSchemes = Scheme)]
        [HttpDelete]
        public IActionResult SoruSil(int id)
        {
            try
            {
                var soru = db.Sorular.Find(id);
                if (soru != null)
                {
                    db.Sorular.Remove(soru);
                    db.SaveChanges();
                    return Json(new { success = true, message = "✅ Soru başarıyla silindi!" });
                }
                return Json(new { success = false, message = "❌ Soru bulunamadı!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "⚠️ Hata oluştu: " + ex.Message });
            }
        }


    }
}
