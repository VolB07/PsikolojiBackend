using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models
{
    public class Service
    {
        public int id { get; set; }

        [Required]
        [MaxLength(255)]
        public string title { get; set; }

        [Required]
        public string description { get; set; }



    }
}

