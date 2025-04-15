using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;

namespace GSBMaas.Controllers
{
    public class KamuKidemTazminatiHesaplama : Controller
    {
        private readonly ILogger<KamuKidemTazminatiHesaplama> _logger;

        public KamuKidemTazminatiHesaplama(ILogger<KamuKidemTazminatiHesaplama> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Kullanıcı giriş yapmış mı kontrol et
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserAd")) ||
                string.IsNullOrEmpty(HttpContext.Session.GetString("UserSoyad")))
            {
                return RedirectToAction("Giris", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Hesapla([FromBody] HesaplamaModel model)
        {
            try
            {
                _logger.LogInformation($"Gelen veriler: Başlangıç: {model.IseBaslamaTarihi}, Bitiş: {model.IstenCikisTarihi}, Ücret: {model.SonBrutUcret}");

                // Tarihleri DateTime'a çevir
                DateTime baslangic = DateTime.Parse(model.IseBaslamaTarihi);
                DateTime bitis = DateTime.Parse(model.IstenCikisTarihi);

                // Toplam çalışma süresini hesapla (gün cinsinden)
                TimeSpan calismaSuresi = bitis - baslangic;
                int toplamGun = calismaSuresi.Days;

                _logger.LogInformation($"Toplam çalışma günü: {toplamGun}");

                // Çıkarılacak günleri topla
                int cikarilacakGun = model.YillikIzinGun + model.MazeretIzinGun + 
                                   model.UcretsizIzinGun + model.RaporGun + (model.AskerlikSure * 30);

                _logger.LogInformation($"Çıkarılacak gün: {cikarilacakGun}");

                // Net çalışma günü
                int netCalismaGun = toplamGun - cikarilacakGun;

                // Yıl hesaplama
                int calismaYili = netCalismaGun / 365;

                _logger.LogInformation($"Çalışma yılı: {calismaYili}");

                // Günlük ücret hesaplama
                decimal gunlukUcret = model.SonBrutUcret / 30;

                // 1 yıllık kıdem tazminatı hesaplama
                decimal birYillikKidemTazminati = gunlukUcret * 30;

                // 2025 yılı için maksimum kıdem tazminatı tutarı (46,655.43 TL)
                decimal maksimumKidemTazminati = 46655.43m;

                // Eğer 1 yıllık kıdem tazminatı maksimum tutarı aşıyorsa, maksimum tutarı kullan
                if (birYillikKidemTazminati > maksimumKidemTazminati)
                {
                    birYillikKidemTazminati = maksimumKidemTazminati;
                }

                // Toplam kıdem tazminatı hesaplama
                decimal toplamKidemTazminati = (birYillikKidemTazminati/365) * netCalismaGun;

                return Json(new { 
                    success = true, 
                    birYillikKidemTazminati = birYillikKidemTazminati.ToString("N2"),
                    toplamKidemTazminati = toplamKidemTazminati.ToString("N2"),
                    calismaYili = calismaYili,
                    netCalismaGun = netCalismaGun
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hesaplama hatası: {ex.Message}");
                return Json(new { success = false, message = "Hesaplama sırasında bir hata oluştu: " + ex.Message });
            }
        }
    }

    public class HesaplamaModel
    {
        public string IseBaslamaTarihi { get; set; }
        public string IstenCikisTarihi { get; set; }
        public decimal SonBrutUcret { get; set; }
        public int YillikIzinGun { get; set; }
        public int MazeretIzinGun { get; set; }
        public int UcretsizIzinGun { get; set; }
        public int RaporGun { get; set; }
        public int AskerlikSure { get; set; }
    }
}
