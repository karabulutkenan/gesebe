using System;
using System.ComponentModel.DataAnnotations;

namespace GSBMaas.Models
{
    public class Soru
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(255)]
        public string Kategori { get; set; }

        [Required]
        public string SoruMetni { get; set; }

        [Required, MaxLength(100)]
        public string SoruSahibi { get; set; }

        [Required]
        public DateTime SoruTarihi { get; set; } = DateTime.Now;

        public string CevapMetni { get; set; } // Cevap nullable olabilir

        public string Cevaplayan { get; set; } // Cevaplayan nullable olabilir

        public DateTime? CevapTarihi { get; set; } // Nullable (Cevap yoksa boş kalabilir)

        public string Kaynak { get; set; } // Cevap verilen kaynağın bilgisi (nullable)

        public bool OnaylandiMi { get; set; } = false; // Cevap onaylanmış mı?
    }
}
