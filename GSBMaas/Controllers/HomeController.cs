﻿using GSBMaas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System;
using GSBMaas.Context;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace GSBMaas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserAd")) ||
                string.IsNullOrEmpty(HttpContext.Session.GetString("UserSoyad")))
            {
                return RedirectToAction("Giris", "Home");
            }
            return View();
        }

        public IActionResult Hesaplamalar()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserAd")) ||
                string.IsNullOrEmpty(HttpContext.Session.GetString("UserSoyad")))
            {
                return RedirectToAction("Giris", "Home");
            }
            return View();
        }

        public async Task<IActionResult> EmekAkademisi()
        {
            var videos = new List<YouTubeVideo>();
            try
            {
                var apiKey = "AIzaSyB7BaHIJcCer26YDvHpiKby9K4cjr_uKD8";
                var channelId = "UCxxxxxxxxxxxxxxxx"; // Toleyis kanal ID'si
                var maxResults = 50;

                using (var client = new HttpClient())
                {
                    // Önce kanal ID'sini al
                    var channelUrl = $"https://www.googleapis.com/youtube/v3/search?part=id&q=@toleyis1977&type=channel&key={apiKey}";
                    var channelResponse = await client.GetStringAsync(channelUrl);
                    var channelData = JsonSerializer.Deserialize<JsonDocument>(channelResponse);
                    
                    if (channelData.RootElement.GetProperty("items").GetArrayLength() > 0)
                    {
                        channelId = channelData.RootElement.GetProperty("items")[0].GetProperty("id").GetProperty("channelId").GetString();
                    }

                    // Kanal videolarını al
                    var videoUrl = $"https://www.googleapis.com/youtube/v3/search?part=snippet&channelId={channelId}&maxResults={maxResults}&order=date&type=video&key={apiKey}";
                    var videoResponse = await client.GetStringAsync(videoUrl);
                    var videoData = JsonSerializer.Deserialize<JsonDocument>(videoResponse);

                    foreach (var item in videoData.RootElement.GetProperty("items").EnumerateArray())
                    {
                        var snippet = item.GetProperty("snippet");
                        videos.Add(new YouTubeVideo
                        {
                            VideoId = item.GetProperty("id").GetProperty("videoId").GetString(),
                            Title = snippet.GetProperty("title").GetString(),
                            Description = snippet.GetProperty("description").GetString(),
                            ThumbnailUrl = snippet.GetProperty("thumbnails").GetProperty("high").GetProperty("url").GetString(),
                            PublishedAt = DateTime.Parse(snippet.GetProperty("publishedAt").GetString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"YouTube API Hatası: {ex.Message}");
            }

            ViewBag.Videos = videos;
            return View();
        }

        public IActionResult KamuMisafirhaneleri()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserAd")) ||
                string.IsNullOrEmpty(HttpContext.Session.GetString("UserSoyad")))
            {
                return RedirectToAction("Giris", "Home");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserAd")) ||
                string.IsNullOrEmpty(HttpContext.Session.GetString("UserSoyad")))
            {
                return RedirectToAction("Giris", "Home");
            }
            return View();
        }

        public IActionResult Sayfa()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserAd")) ||
                string.IsNullOrEmpty(HttpContext.Session.GetString("UserSoyad")))
            {
                return RedirectToAction("Giris", "Home");
            }
            return View();
        }

        public IActionResult DilekceOrnekleri()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserAd")) ||
                string.IsNullOrEmpty(HttpContext.Session.GetString("UserSoyad")))
            {
                return RedirectToAction("Giris", "Home");
            }
            return View();
        }

        public IActionResult KanunVeYonetmelikler()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserAd")) ||
                string.IsNullOrEmpty(HttpContext.Session.GetString("UserSoyad")))
            {
                return RedirectToAction("Giris", "Home");
            }
            return View();
        }

        public IActionResult Giris()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // ✅ **Yeni GirisKontrol Metodu - HttpClient DI'siz Kullanıldı**
        [HttpPost]
        public async Task<IActionResult> GirisKontrol([FromBody] GirisModel model)
        {
            string apiUrl = "https://toleyis.agembilisim.com.tr/asya/ws/uye/select.do"; // API URL
            string apiKey = "8701027b-8542-4dc8-8852-edf73ff180e3"; // Rest API Key
            string username = "rest.service"; // Kullanıcı Adı
            string password = "uP7G=76q"; // Şifre

            var requestBody = new
            {
                tcKimlikNo = model.TcKimlik,
                dogrulamaKodu = model.UyelikKodu
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // ✅ **DÜZELTME: `Content-Type` başlığını `HttpContent.Headers` içine ekledik**
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Rest-Api-Key", apiKey);
                httpClient.DefaultRequestHeaders.Add("Username", username);
                httpClient.DefaultRequestHeaders.Add("Password", password);

                Console.WriteLine("📤 API'ye Gönderilen Veri: " + json); // 📌 API'ye gönderilen veriyi logla

                var response = await httpClient.PostAsync(apiUrl, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                Console.WriteLine("📥 API Cevabı: " + jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);

                    if (result.code == 200 && result.result != null && result.result.uye == "E") // ✅ API başarılı döndü mü kontrol et
                    {
                        HttpContext.Session.SetString("UserLoggedIn", "true");
                        HttpContext.Session.SetString("UserTc", model.TcKimlik);
                        HttpContext.Session.SetString("UserAd", result.result.ad);
                        HttpContext.Session.SetString("UserSoyad", result.result.soyad);
                        HttpContext.Session.SetString("Unvan", result.result.unvan);
                        HttpContext.Session.SetString("Subesi", result.result.sube);
                        HttpContext.Session.SetString("UyelikTarihi", result.result.uyelikBaslangicTarihi);
                        HttpContext.Session.SetString("SgkSicilNo", result.result.sgkSicilNo);

                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Üyelik doğrulaması başarısız!" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "API bağlantı hatası! HTTP Status Code: " + response.StatusCode });
                }
            }
        }

        [HttpPost]
        public IActionResult GirisKontrolModerator([FromBody] GirisModel model)
        {
            using (var db = new AppDbContext()) // Veritabanına bağlan
            {
                var moderator = db.Moderatorler.FirstOrDefault(m => m.KullaniciAdi == model.TcKimlik); // Kullanıcı adını bul

                if (moderator != null && BCrypt.Net.BCrypt.Verify(model.UyelikKodu, moderator.SifreHash)) // Şifreyi doğrula
                {
                    // Giriş başarılı, Session'a bilgileri ekleyelim
                    HttpContext.Session.SetString("UserLoggedIn", "true");
                    HttpContext.Session.SetString("UserAd", moderator.Ad);
                    HttpContext.Session.SetString("UserSoyad", moderator.Soyad);
                    HttpContext.Session.SetString("KullaniciAdi", moderator.KullaniciAdi);
                    return Json(new { success = true });
                }
            }

            // Eğer giriş başarısızsa hata mesajı gönder
            return Json(new { success = false, message = "Moderatör girişi başarısız!" });
        }

        public IActionResult TestSession()
        {
            var userAd = HttpContext.Session.GetString("UserAd");
            var userSoyad = HttpContext.Session.GetString("UserSoyad");

            Console.WriteLine("Session Test: Kullanıcı Adı - " + userAd);
            Console.WriteLine("Session Test: Kullanıcı Soyadı - " + userSoyad);

            return Json(new { Ad = userAd, Soyad = userSoyad });
        }

        [HttpPost]
        public IActionResult Cikis()
        {
            HttpContext.Session.Clear(); // ✅ Kullanıcı bilgilerini temizle
            return RedirectToAction("Giris"); // ✅ Giriş sayfasına yönlendir
        }

        [HttpGet]
        public IActionResult DownloadPdf(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return BadRequest("Dosya yolu belirtilmedi.");

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
            
            if (!System.IO.File.Exists(fullPath))
                return NotFound("Dosya bulunamadı.");

            var memory = new MemoryStream();
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            return File(memory, "application/pdf", Path.GetFileName(fullPath));
        }
    }

    public class GirisModel
    {
        public string TcKimlik { get; set; }
        public string UyelikKodu { get; set; }
    }

    public class ApiResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public string tcKimlikNo { get; set; }
        public string dogrulamaKodu { get; set; }
        public string ad { get; set; }
        public string soyad { get; set; }
        public string uye { get; set; }
        public string unvan { get; set; }
        public string sube { get; set; }
        public string uyelikBaslangicTarihi { get; set; }
        public string sgkSicilNo { get; set; }
    }
}
