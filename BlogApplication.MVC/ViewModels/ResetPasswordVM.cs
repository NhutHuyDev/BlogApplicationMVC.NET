using System.ComponentModel.DataAnnotations;

namespace BlogApplication.MVC.ViewModels
{
    public class ResetPasswordVM
    {
        public string Id { get; set; } = String.Empty;
        public string UserName { get; set; } = String.Empty;
        public string NewPassword { get; set; } = String.Empty;

        [Compare(nameof(NewPassword), ErrorMessage= "Confirm password doest not match")]        
        public string ConfirmPassword { get; set; } = String.Empty;
    }
}
