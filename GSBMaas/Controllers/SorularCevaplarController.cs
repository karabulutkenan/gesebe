using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GSBMaas.Context;
using GSBMaas.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace GSBMaas.Controllers
{
    public class SorularCevaplarController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserAd")) ||
                string.IsNullOrEmpty(HttpContext.Session.GetString("UserSoyad")))
            {
                return RedirectToAction("Giris", "Home");
            }

            // Sadece misafir kullanıcı kontrolü
            if (HttpContext.Session.GetString("MisafirKullanici") == "true")
            {
                return RedirectToAction("Index", "Home");
            }

            // ✅ Yalnızca onaylanmış soruları listele
            ViewBag.SoruKategoriler = db.SoruKategoriler.ToList();
            ViewBag.Sorular = db.Sorular.Where(s => s.OnaylandiMi).OrderBy(s => s.SoruMetni).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult SoruEkle(int KategoriId, string SoruMetni, string SoruSoran, string Eposta)
        {
            try
            {
                var yeniSoru = new Soru
                {
                    KategoriId = KategoriId,
                    SoruMetni = SoruMetni,
                    CevapMetni = "Henüz cevaplanmadı.",
                    Cevaplayan = "Beklemede",
                    CevaplayanMail = Eposta,
                    Kaynak = "-",
                    SoruTarihi = DateTime.Now,
                    CevapTarihi = null,
                    OnaylandiMi = false,
                    SoruSoran = SoruSoran
                };
                db.Sorular.Add(yeniSoru);
                db.SaveChanges();
                bool emailSent = SoruSoranaEpostaGonder(Eposta, SoruMetni);
                return Json(new
                {
                    success = true,
                    message = emailSent
                        ? "✅ Sorunuz başarıyla gönderildi, onay bekliyor! E-posta adresinize bilgilendirme gönderildi."
                        : "✅ Sorunuz başarıyla gönderildi, ancak e-posta gönderilemedi."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "⚠️ Hata oluştu: " + ex.Message });
            }
        }

        private bool SoruSoranaEpostaGonder(string eposta, string soruMetni)
        {
            try
            {
                var fromAddress = new MailAddress("kenan@toleyis.org.tr", "E-Sendika");
                var toAddress = new MailAddress(eposta);
                const string subject = "E-sendika Sorulan Soru";
                string body = $"Sorunuz: \"{soruMetni}\"\n\nSorunuz sistemimize ulaşmıştır.";
                using (var smtp = new SmtpClient("mail.kurumsaleposta.com", 587)
                {
                    EnableSsl = false,
                    Credentials = new NetworkCredential("kenan@toleyis.org.tr", "Melis2604K25"),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 20000 // 20 saniye timeout
                })
                {
                    smtp.Send(fromAddress.Address, toAddress.Address, subject, body);
                }
                Console.WriteLine($"✅ E-posta başarıyla gönderildi: {eposta}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ E-posta gönderme hatası: {ex.Message}");
                return false;
            }
        }
    }
}