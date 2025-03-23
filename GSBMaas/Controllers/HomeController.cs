using GSBMaas.Models;
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
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

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

        [HttpPost]
        public IActionResult MisafirGiris([FromBody] MisafirKullanici model)
        {
            try
            {
                // Gelen veriyi logla
                _logger.LogInformation($"Misafir girişi denemesi - Email: {model.Email}, Ad: {model.Ad}, Soyad: {model.Soyad}");

                if (model == null)
                {
                    _logger.LogError("Model null geldi");
                    return Json(new { success = false, message = "Geçersiz veri" });
                }

                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Ad) || string.IsNullOrEmpty(model.Soyad))
                {
                    _logger.LogError($"Eksik veri - Email: {model.Email}, Ad: {model.Ad}, Soyad: {model.Soyad}");
                    return Json(new { success = false, message = "Lütfen tüm alanları doldurunuz" });
                }

                using (var db = new AppDbContext())
                {
                    try
                    {
                        // Email kontrolü
                        _logger.LogInformation("Veritabanı kontrolü başlıyor");
                        var mevcutKullanici = db.MisafirKullanicilar.FirstOrDefault(m => m.Email == model.Email);
                        
                        if (mevcutKullanici != null)
                        {
                            _logger.LogInformation($"Mevcut kullanıcı bulundu: {mevcutKullanici.Email}");
                            // Kullanıcı varsa, son giriş tarihini güncelle
                            mevcutKullanici.SonGirisTarihi = DateTime.Now;
                            db.SaveChanges();
                        }
                        else
                        {
                            _logger.LogInformation("Yeni kullanıcı oluşturuluyor");
                            // Yeni kullanıcı oluştur
                            model.SonGirisTarihi = DateTime.Now;
                            db.MisafirKullanicilar.Add(model);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception dbEx)
                    {
                        _logger.LogError($"Veritabanı hatası: {dbEx.Message}");
                        throw;
                    }
                }

                // Doğrulama kodu oluştur
                _logger.LogInformation("Doğrulama kodu oluşturuluyor");
                var dogrulamaKodu = GenerateVerificationCode();

                try
                {
                    // Email gönder
                    _logger.LogInformation($"Email gönderiliyor: {model.Email}");
                    SendVerificationEmail(model.Email, dogrulamaKodu);
                }
                catch (Exception emailEx)
                {
                    _logger.LogError($"Email gönderme hatası: {emailEx.Message}");
                    _logger.LogError($"Email gönderme hatası detay: {emailEx.StackTrace}");
                    return Json(new { success = false, message = "Email gönderilemedi: " + emailEx.Message });
                }

                // Doğrulama kodunu session'da sakla
                HttpContext.Session.SetString("MisafirDogrulamaKodu", dogrulamaKodu);
                HttpContext.Session.SetString("MisafirEmail", model.Email);
                HttpContext.Session.SetString("MisafirAd", model.Ad);
                HttpContext.Session.SetString("MisafirSoyad", model.Soyad);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Misafir girişi genel hata: {ex.Message}");
                _logger.LogError($"Hata detayı: {ex.StackTrace}");
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult MisafirDogrulama([FromBody] MisafirDogrulamaModel model)
        {
            try
            {
                var sessionKod = HttpContext.Session.GetString("MisafirDogrulamaKodu");
                var sessionEmail = HttpContext.Session.GetString("MisafirEmail");
                var sessionAd = HttpContext.Session.GetString("MisafirAd");
                var sessionSoyad = HttpContext.Session.GetString("MisafirSoyad");

                if (string.IsNullOrEmpty(sessionKod) || string.IsNullOrEmpty(sessionEmail) || 
                    string.IsNullOrEmpty(sessionAd) || string.IsNullOrEmpty(sessionSoyad))
                {
                    return Json(new { success = false, message = "Oturum süresi dolmuş. Lütfen tekrar deneyiniz." });
                }

                if (model.Email != sessionEmail)
                {
                    return Json(new { success = false, message = "E-posta adresi eşleşmiyor." });
                }

                if (model.Kod != sessionKod)
                {
                    return Json(new { success = false, message = "Geçersiz doğrulama kodu." });
                }

                // Giriş başarılı, session'ı güncelle
                HttpContext.Session.SetString("UserLoggedIn", "true");
                HttpContext.Session.SetString("UserAd", sessionAd);
                HttpContext.Session.SetString("UserSoyad", sessionSoyad);
                HttpContext.Session.SetString("MisafirKullanici", "true");

                // Session'daki geçici bilgileri temizle
                HttpContext.Session.Remove("MisafirDogrulamaKodu");
                HttpContext.Session.Remove("MisafirEmail");
                HttpContext.Session.Remove("MisafirAd");
                HttpContext.Session.Remove("MisafirSoyad");

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Misafir doğrulama hatası: {ex.Message}");
                return Json(new { success = false, message = "Bir hata oluştu. Lütfen tekrar deneyiniz." });
            }
        }

        private string GenerateVerificationCode()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[4]; // 3 yerine 4 byte kullanacağız
                rng.GetBytes(bytes);
                var number = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 1000000; // 6 haneli sayı için mod 1000000
                return number.ToString("D6"); // Her zaman 6 haneli olmasını sağla
            }
        }

        private void SendVerificationEmail(string email, string code)
        {
            try
            {
                _logger.LogInformation($"Email gönderme başlıyor - Alıcı: {email}");

                var fromAddress = new MailAddress("kenan@toleyis.org.tr", "E-Sendika");
                var toAddress = new MailAddress(email);
                const string subject = "GSB Maaş - Misafir Girişi Doğrulama Kodu";
                string body = $"Merhaba,\n\nMisafir girişi için doğrulama kodunuz: {code}\n\nBu kod 5 dakika süreyle geçerlidir.\n\nSaygılarımızla,\nGSB Maaş Ekibi";

                _logger.LogInformation("SMTP client oluşturuluyor");
                using (var smtp = new SmtpClient("mail.kurumsaleposta.com", 587)
                {
                    EnableSsl = false,
                    Credentials = new NetworkCredential("kenan@toleyis.org.tr", "Melis2604K25"),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 20000 // 20 saniye timeout
                })
                {
                    _logger.LogInformation("Email gönderiliyor...");
                    smtp.Send(fromAddress.Address, toAddress.Address, subject, body);
                    _logger.LogInformation($"✅ Email başarıyla gönderildi: {email}");
                }
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError($"SMTP Hatası: {smtpEx.Message}");
                _logger.LogError($"SMTP Hata Kodu: {smtpEx.StatusCode}");
                _logger.LogError($"SMTP Hata Detayı: {smtpEx.StackTrace}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Email gönderme hatası: {ex.Message}");
                _logger.LogError($"Hata detayı: {ex.StackTrace}");
                throw;
            }
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

    public class MisafirDogrulamaModel
    {
        public string Email { get; set; }
        public string Kod { get; set; }
    }
}
