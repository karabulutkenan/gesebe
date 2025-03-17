using GSBMaas.Context;
using GSBMaas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GSBMaas.Controllers
{
    public class DijitalKimlikController : Controller
    {
        private readonly AppDbContext _context;

        public DijitalKimlikController()
        {
            _context = new AppDbContext();
        }

        public IActionResult Index()
        {
            // Kullanıcı giriş kontrolü
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserAd")) ||
                string.IsNullOrEmpty(HttpContext.Session.GetString("UserSoyad")))
            {
                return RedirectToAction("Giris", "Home");
            }

            // Session'dan kullanıcı bilgilerini al
            var tcNo = HttpContext.Session.GetString("UserTc");
            var ad = HttpContext.Session.GetString("UserAd");
            var soyad = HttpContext.Session.GetString("UserSoyad");
            var unvan = HttpContext.Session.GetString("Unvan");
            var sube = HttpContext.Session.GetString("Subesi");
            var uyelikBaslangicTarihi = HttpContext.Session.GetString("UyelikTarihi");
            var sgkSicilNo = HttpContext.Session.GetString("SgkSicilNo");

            // Sadece fotoğraf bilgisi için veritabanına bakıyoruz
            var userProfile = _context.UserProfiles.FirstOrDefault(x => x.TcNo == tcNo);

            ViewBag.Ad = ad;
            ViewBag.Soyad = soyad;
            ViewBag.Unvan = unvan;
            ViewBag.TcNo = tcNo;
            ViewBag.Subesi = sube;
            ViewBag.UyelikTarihi = uyelikBaslangicTarihi;
            ViewBag.SgkSicilNo = sgkSicilNo;
            ViewBag.PhotoPath = userProfile?.PhotoPath ?? "/images/default-avatar.png";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
                return Json(new { success = false, message = "Lütfen bir fotoğraf seçiniz." });

            try
            {
                var tcNo = HttpContext.Session.GetString("UserTc");
                if (string.IsNullOrEmpty(tcNo))
                {
                    return Json(new { success = false, message = "Oturum bilgisi bulunamadı. Lütfen tekrar giriş yapın." });
                }

                var userProfile = _context.UserProfiles.FirstOrDefault(x => x.TcNo == tcNo);

                // Dosya uzantısını kontrol et
                var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    return Json(new { success = false, message = "Sadece .jpg, .jpeg veya .png formatında dosya yükleyebilirsiniz." });

                // Dosya adını oluştur
                var fileName = $"{tcNo}_{DateTime.Now.Ticks}.jpg";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "photos", fileName);

                // Uploads klasörünü oluştur
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "photos");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Dosyayı kaydet
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                // Veritabanını güncelle
                if (userProfile == null)
                {
                    userProfile = new UserProfile
                    {
                        TcNo = tcNo,
                        PhotoPath = $"/uploads/photos/{fileName}",
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };
                    _context.UserProfiles.Add(userProfile);
                }
                else
                {
                    // Eski fotoğrafı sil
                    if (!string.IsNullOrEmpty(userProfile.PhotoPath))
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", userProfile.PhotoPath.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    userProfile.PhotoPath = $"/uploads/photos/{fileName}";
                    userProfile.UpdatedDate = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, photoPath = $"/uploads/photos/{fileName}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Fotoğraf yüklenirken bir hata oluştu: " + ex.Message });
            }
        }
    }
} 