using AspNetCoreHero.ToastNotification.Abstractions;
using BlogApplication.MVC.Data;
using BlogApplication.MVC.Models;
using BlogApplication.MVC.Utilities;
using BlogApplication.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostController : Controller
    {
        private readonly BlogApplicationContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService _notification;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController (BlogApplicationContext context, 
                        IWebHostEnvironment webHostEnvironment, 
                        INotyfService notification,
                        UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _notification = notification;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Post", "Dashboard", new { area = "Admin" });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePostVM());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostVM vm)
        {
            if (!ModelState.IsValid){
                return View(vm);
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

            var post = new Post()
            {
                Title = vm.Title,
                Content = vm.Content,
                ShortDescription = vm.ShortDescription,
                AuthorId = loggedInUser!.Id
            };

            string slug = vm.Title.Trim();
            slug = slug.Replace(" ", "-");
            post.Slug = slug + "-" + Guid.NewGuid();

            FileHandler fileHandler = new FileHandler(_webHostEnvironment);
            post.ThumbnailUrl = fileHandler.UploadImage(vm.Thumbnail);

            await _context.Posts!.AddAsync(post);
            await _context.SaveChangesAsync();

            _notification.Success("Post Created Successfully");
            return RedirectToAction("Post", "Dashboard", new {area = "Admin"});
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete (int Id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == Id);

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin || loggedInUser?.Id == post?.AuthorId)
            {
                _context.Posts!.Remove(post!);
                await _context.SaveChangesAsync();
                _notification.Success("Post Deleted Successfully");
                return RedirectToAction("Index", "Post", new { area = "Admin" });
            } else
            {
                _notification.Error("do not have the permission to delete");
                return View();
            }

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == Id);
            if (post == null)
            {
                _notification.Error("Post not found");
                return View();
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);
            if (loggedInUserRole[0] != WebsiteRoles.WebsiteAdmin && loggedInUser!.Id != post.AuthorId)
            {
                _notification.Error("You are not authorized");
                return RedirectToAction("Index");
            }

            var vm = new EditPostVM()
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Content = post.Content,
                ThumbnailUrl = post.ThumbnailUrl,
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditPostVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (post == null)
            {
                _notification.Error("Post not found");
                return View();
            }

            post.Title = vm.Title;
            post.ShortDescription = vm.ShortDescription;
            post.Content = vm.Content;

            if (vm.Thumbnail != null)
            {
                FileHandler fileHandler = new FileHandler(_webHostEnvironment);
                post.ThumbnailUrl = fileHandler.UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            _notification.Success("Post updated succesfully");
            return RedirectToAction("Index", "Post", new { area = "Admin" });
        }
    }
}
