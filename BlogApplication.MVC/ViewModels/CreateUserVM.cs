using System.ComponentModel.DataAnnotations;

namespace BlogApplication.MVC.ViewModels
{
    public class CreateUserVM
    {
        [Required]
        public string FirstName { get; set; } = String.Empty;
        
        [Required]
        public string LastName { get; set; } = String.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = String.Empty;

        [Required]
        public string UserName { get; set; } = String.Empty;

        [Required]
        public string Password { get; set; } = String.Empty;

        [Required]
        public string ConfirmPassword { get; set; } = String.Empty;

        [Required]
        public bool IsAdmin { get; set; } 
    }
}
