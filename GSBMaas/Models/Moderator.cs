using System.ComponentModel.DataAnnotations;

namespace GSBMaas.Models
{
    public class Moderator
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(50)]
        public string Ad { get; set; }

        [Required, MaxLength(50)]
        public string Soyad { get; set; }

        [Required, MaxLength(50)]
        public string KullaniciAdi { get; set; }

        [Required, MaxLength(255)] // Şifreler hashlenmiş şekilde saklanacak
        public string SifreHash { get; set; }
    }
}
