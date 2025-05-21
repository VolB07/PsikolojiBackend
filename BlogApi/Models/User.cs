namespace BlogApi.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int id { get; set; }

        [Required]
        [MaxLength(50)]
        public string username { get; set; } = string.Empty;

        [Required]
        public string password_hash { get; set; } = string.Empty;

        [Required]
        public string role { get; set; } = "viewer"; // Varsayılan role
    }

}
