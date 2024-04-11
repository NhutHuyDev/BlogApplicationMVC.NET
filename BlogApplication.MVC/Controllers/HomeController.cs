using BlogApplication.MVC.Data;
using BlogApplication.MVC.Models;
using BlogApplication.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BlogApplication.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, BlogApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string page = "1")
        {
            int numPostPerPage = 2;

            var posts = _context.Posts.Include(post => post.Author);

            int totalPage = (int)Math.Ceiling((double)posts.Count() / numPostPerPage);

            if (!int.TryParse(page, out int currentPage))
            {
                return RedirectToAction("NotFound");
            } 

            if(currentPage <= 0 | currentPage > totalPage)
            {
                return RedirectToAction("NotFound");
            }

            var filterPosts = posts.OrderByDescending(post =>post.CreatedDate)
                                    .Skip(numPostPerPage * (currentPage - 1))
                                    .Take(numPostPerPage);

            List<PostVM> postsVM = filterPosts.Select(x => new PostVM()
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                ThumbnailUrl = x.ThumbnailUrl,
                ShortDescription = x.ShortDescription,
                AuthorName = x.Author!.FirstName + " " + x.Author.LastName
            }).ToList();

            HomeVM homeVM = new()
            {
                postsVM = postsVM,
                currentPage = currentPage,
                totalPages = totalPage
            };

            return View(homeVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/StatusCodeError/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            if (statusCode == 404)
            {
                return RedirectToAction("NotFound");
            }

            return RedirectToAction("InternalServerError");
        }

        [HttpGet("/NotFound")]
        public IActionResult NotFound()
        {
            return View();
        }

        public IActionResult InternalServerError()
        {
            return View();
        }

    }
}
