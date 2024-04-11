using BlogApplication.MVC.Data;
using BlogApplication.MVC.Models;
using BlogApplication.MVC.Utilities;
using BlogApplication.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using X.PagedList;

namespace BlogApplication.MVC.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly BlogApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(BlogApplicationContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                ViewData["controller"] = "Index";
                return View();
            } else
            {
                return RedirectToAction("Post");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Post(int? page)
        {
            var Posts = new List<Post>();
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == HttpContext.User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin)
            {
                Posts = await _context.Posts!.Include(x => x.Author).ToListAsync();
            }
            else
            {
                Posts = await _context.Posts!.Include(x => x.Author).Where(x => x.Author!.Id == loggedInUser!.Id).ToListAsync();
            }

            var listOfPostsVM = Posts.Select(x => new PostVM()
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                ThumbnailUrl = x.ThumbnailUrl,
                AuthorName = x.Author!.FirstName + " " + x.Author.LastName
            }).ToList();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            ViewData["controller"] = "Post";
            return View(await listOfPostsVM.OrderByDescending(x => x.CreatedDate).ToPagedListAsync(pageNumber, pageSize));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]   
        public async Task<IActionResult> User()
        {
            var users = await _userManager.Users.ToListAsync();

            var vm = users.Select(user => new UserVM()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            }).ToList();

            foreach (var user in vm)
            {
                var singleUser = await _userManager.FindByIdAsync(user.Id);
                var roles = await _userManager.GetRolesAsync(singleUser);
                user.Role = roles.FirstOrDefault();
            }

            ViewData["controller"] = "User";
            return View(vm);
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
