namespace BlogApplication.MVC.ViewModels
{
    public class HomeVM
    {
        public List<PostVM>? postsVM { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
    }
}
