using System.ComponentModel.DataAnnotations;

namespace BlogApplication.MVC.ViewModels
{
    public class CreatePostVM
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        public string? Content { get; set; }

        public string? AuthorId { get; set; }

        public string? Slug { get; set; }

        [Required]
        public IFormFile Thumbnail { get; set; }
    }
}
