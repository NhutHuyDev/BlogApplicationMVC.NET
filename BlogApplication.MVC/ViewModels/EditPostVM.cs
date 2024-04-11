using System.ComponentModel.DataAnnotations;

namespace BlogApplication.MVC.ViewModels
{
    public class EditPostVM
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? ShortDescription { get; set; }

        public string? Content { get; set; }

        public string? AuthorId { get; set; }

        public string? ThumbnailUrl { get; set; }

        public IFormFile? Thumbnail { get; set; }
    }
}
