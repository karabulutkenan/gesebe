using System;
using System.ComponentModel.DataAnnotations;

namespace GSBMaas.Models
{
    public class MisafirKullanici
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Ad { get; set; }

        [Required]
        public string Soyad { get; set; }

        public DateTime SonGirisTarihi { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
    }
} 