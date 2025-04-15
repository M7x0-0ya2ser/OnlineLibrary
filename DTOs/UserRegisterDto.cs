using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        public string Username { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateOnly? Dateofbirth { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; }
        [Required]
        public string Repassword { get; set; }
    }
}
