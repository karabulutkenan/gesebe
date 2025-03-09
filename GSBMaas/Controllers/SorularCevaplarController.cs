using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GSBMaas.Context;
using GSBMaas.Models;
using System;
using System.Linq;

namespace GSBMaas.Controllers
{
    public class SorularCevaplarController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        public IActionResult Index()
        {
            // Kullanıcı giriş yapmış mı kontrol et
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserAd")) ||
                string.IsNullOrEmpty(HttpContext.Session.GetString("UserSoyad")))
            {
                return RedirectToAction("Giris", "Home");
            }

            // ✅ Yalnızca onaylanmamış soruları listele
            ViewBag.SoruKategoriler = db.SoruKategoriler.ToList();
            ViewBag.Sorular = db.Sorular.Where(s => s.OnaylandiMi).OrderBy(s => s.SoruMetni).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult SoruEkle(int KategoriId, string SoruMetni, string SoruSoran)
        {
            try
            {
                if (KategoriId <= 0 || string.IsNullOrEmpty(SoruMetni))
                {
                    return Json(new { success = false, message = "⚠️ Lütfen bir kategori seçin ve soruyu doldurun!" });
                }

                var yeniSoru = new Soru
                {
                    KategoriId = KategoriId,
                    SoruMetni = SoruMetni,
                    CevapMetni = "Henüz cevaplanmadı.",
                    Cevaplayan = "Beklemede",
                    Kaynak = "-",
                    SoruTarihi = DateTime.Now,
                    CevapTarihi = null,
                    OnaylandiMi = false, // ✅ Onaylanmamış olarak eklenecek
                    SoruSoran = SoruSoran,
                    
                };

                db.Sorular.Add(yeniSoru);
                db.SaveChanges();

                return Json(new { success = true, message = "✅ Sorunuz başarıyla gönderildi, onay bekliyor!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "⚠️ Hata oluştu: " + ex.Message });
            }
        }

    }
}
