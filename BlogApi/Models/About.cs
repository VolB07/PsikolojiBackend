using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models
{
    public class About
    {
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string university { get; set; }

        public string experience { get; set; }

        public string description { get; set; }

        public string image_url { get; set; }
    }
}


