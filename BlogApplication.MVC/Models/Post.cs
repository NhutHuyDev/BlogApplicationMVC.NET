using System.Numerics;

namespace BlogApplication.MVC.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }
      
        public string ShortDescription { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; } 

        public ApplicationUser Author { get; set; } 

        public string Slug { get; set; } 

        public string ThumbnailUrl { get; set; } 
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
