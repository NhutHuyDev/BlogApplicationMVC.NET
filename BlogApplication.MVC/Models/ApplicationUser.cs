using Microsoft.AspNetCore.Identity;

namespace BlogApplication.MVC.Models
{
    public class ApplicationUser :  IdentityUser
    {
        public string FirstName { get; set; } = String.Empty;
        
        public string LastName { get; set; } = String.Empty;

        public IList<Post>? Posts { get; set; }
    }
}
