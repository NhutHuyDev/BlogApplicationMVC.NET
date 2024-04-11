using AspNetCoreHero.ToastNotification.Abstractions;
using BlogApplication.MVC.Models;
using BlogApplication.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace BlogApplication.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly INotyfService _notification;

        public AuthController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                INotyfService notification
                            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notification = notification;
        }

        public IActionResult Index()
        {
            return Redirect("/Login");
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return View(new LoginVM());
            }

            return RedirectToAction("Index", "Dashboard", new {area = "Admin"});
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }

            var existingUsername = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == vm.Username);
            if (existingUsername == null)
            {
                ModelState.AddModelError("Password", "Username or Password is not correct");
                return View(vm);
            }

            var verifyPassword = await _userManager.CheckPasswordAsync(existingUsername, vm.Password);
            if (!verifyPassword)
            {
                ModelState.AddModelError("Password", "Username or Password is not correct");
                return View(vm);
            }

            await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, true);

            _notification.Success("Login successfully");

            return RedirectToAction("Index", "Dashboard", new {area = "Admin"});
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _notification.Success("Logout successfully");
            return RedirectToAction("Index", "Auth", new { area = "Admin" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            var vm = new ResetPasswordVM()
            {
                Id = existingUser.Id,
                UserName = existingUser.UserName
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var existingUser = await _userManager.FindByIdAsync(vm.Id);
            if (existingUser == null)
            {
                _notification.Error("User Not Found");
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);

            var result = await _userManager.ResetPasswordAsync(existingUser, token, vm.NewPassword);

            if (result.Succeeded)
            {
                _notification.Success($"Rest passwrod for {vm.UserName} successfully");
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            } else
            {
                var messages = result.Errors.Select(x => x.Description).FirstOrDefault();
                ModelState.AddModelError("ConfirmPassword", messages);
                return View(vm);
            }
        }
    }
}

