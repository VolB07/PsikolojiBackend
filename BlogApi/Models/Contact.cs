using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models
{
    public class Contact
    {
        public int id { get; set; }

        [Required]
        [MaxLength(255)]
        public string address { get; set; }

        [Required]
        [MaxLength(50)]
        public string whatsapp { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }
    }
}
