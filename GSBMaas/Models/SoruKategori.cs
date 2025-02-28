using System.ComponentModel.DataAnnotations;

namespace GSBMaas.Models
{
    public class SoruKategori
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Ad { get; set; }
    }
}
