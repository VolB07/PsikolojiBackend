using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models
{
    public class Blog
    {
        public int id { get; set; }

        [Required]
        [MaxLength(255)]
        public string title { get; set; }

        [Required]
        public string summary { get; set; }

        [Required]
        public string content { get; set; }

        [Required]
        public string image_url { get; set; }

        public DateTime created_at { get; set; } = DateTime.UtcNow;
    }
}

