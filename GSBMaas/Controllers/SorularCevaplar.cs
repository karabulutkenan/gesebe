using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSBMaas.Controllers
{
    public class SorularCevaplar : Controller
    {
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
    }
}
