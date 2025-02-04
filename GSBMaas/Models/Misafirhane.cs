using System.ComponentModel.DataAnnotations;

namespace GSBMaas.Models
{
    public class Misafirhane
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Adi { get; set; }

        [Required]
        [StringLength(50)]
        public string Ili { get; set; }

        [Required]
        [Phone]
        public string SabitTelefon { get; set; }

        [Phone]
        public string CepTelefon { get; set; }

        [Required]
        [StringLength(250)]
        public string Adres { get; set; }
    }
}
