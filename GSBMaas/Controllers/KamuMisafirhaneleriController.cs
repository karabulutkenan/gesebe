﻿using GSBMaas.Context;
using GSBMaas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GSBMaas.Controllers
{
    public class KamuMisafirhaneleriController : Controller
    {
        AppDbContext db = new AppDbContext(); // DI Kullanılmadan Bağlantı Tanımlandı

        public IActionResult Index()
        {
            try
            {
                // **Veritabanında kayıtlı olan illeri alıyoruz**
                var iller = db.Misafirhaneler
                              .Where(m => !string.IsNullOrEmpty(m.Ili))
                              .Select(m => m.Ili)
                              .Distinct()
                              .OrderBy(i => i)
                              .ToList();

                ViewBag.Iller = iller;

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

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA: {ex.Message}");
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult GetMisafirhaneler(string il, string kelime)
        {
            try
            {
                var misafirhaneler = db.Misafirhaneler.AsQueryable();

                if (!string.IsNullOrEmpty(il))
                {
                    misafirhaneler = misafirhaneler.Where(m => m.Ili == il);
                }

                if (!string.IsNullOrEmpty(kelime))
                {
                    misafirhaneler = misafirhaneler.Where(m => m.Adi.Contains(kelime));
                }

                var sonuc = misafirhaneler
                    .Select(m => new
                    {
                        adi = m.Adi,
                        il = m.Ili,
                        telefon = string.IsNullOrEmpty(m.CepTelefon) ? m.SabitTelefon : m.CepTelefon,
                        adres = m.Adres
                    })
                    .OrderBy(m => m.adi)
                    .ToList();

                return Json(sonuc);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HATA: {ex.Message}");
                return Json(new { hata = "Veri çekilemedi!" });
            }
        }
    }
}
