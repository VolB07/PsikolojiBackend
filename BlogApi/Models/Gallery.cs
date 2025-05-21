using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models
{
    public class Gallery
    {
        public int id { get; set; }

        [Required]
        public string image_url { get; set; }

        [MaxLength(255)]
        public string? alt_text { get; set; }  // Nullable ve Required kaldırıldı
    }
}
