using Microsoft.AspNetCore.Mvc;

namespace GSBMaas.Controllers
{
    public class SorularCevaplar : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
