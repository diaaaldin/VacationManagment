using System.ComponentModel.DataAnnotations;

namespace VacationManagment.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "confirm your password")]
        public string PasswordConfirm { get; set; }
    }
}
