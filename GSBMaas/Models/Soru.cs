using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSBMaas.Models
{
    public class Soru
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("SoruKategori")]
        public int KategoriId { get; set; }
        public SoruKategori SoruKategori { get; set; }

        [Required, MaxLength(500)]
        public string SoruMetni { get; set; }

        [MaxLength(2000)]
        public string CevapMetni { get; set; }

        [Required, MaxLength(100)]
        public string SoruSoran { get; set; }

        [Required, MaxLength(100)]
        public string Cevaplayan { get; set; }

        [MaxLength(500)]
        public string Kaynak { get; set; }

        public DateTime SoruTarihi { get; set; } = DateTime.Now;

        public DateTime? CevapTarihi { get; set; }

        public bool OnaylandiMi { get; set; } = false; // ✅ Onaylanmamış olarak eklenecek
    }
}
