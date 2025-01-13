using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace GSBMaas.Controllers
{
    public class OrnekController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ResponsiveTest()
        {
            return View();
        }

        public IActionResult ToplamaIslemi(int sayi1, int sayi2)
        {
            var islem = sayi1 + sayi2;
            var toplam = JsonConvert.SerializeObject(islem).ToString();
            return Json(toplam);
        }
    }
}
