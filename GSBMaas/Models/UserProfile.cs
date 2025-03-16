using System;
using System.ComponentModel.DataAnnotations;

namespace GSBMaas.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "TC Kimlik No zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 haneli olmalıdır")]
        public string TcNo { get; set; }

        public string PhotoPath { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
} 