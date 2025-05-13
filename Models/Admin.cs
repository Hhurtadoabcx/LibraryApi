using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class Admin
    {
        public int AdminId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
