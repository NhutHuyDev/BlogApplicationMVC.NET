using System.ComponentModel.DataAnnotations;

namespace BlogApplication.MVC.ViewModels
{
    public class LoginVM
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Username { get; set; } = String.Empty;
        [Required]
        public string Password { get; set; } = String.Empty;
        public bool RememberMe { get; set; }
    }
}
