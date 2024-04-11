using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using BlogApplication.MVC.Data;
using BlogApplication.MVC.Models;
using BlogApplication.MVC.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BlogApplicationContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("BlogApplicationContext")
                ?? throw new InvalidOperationException("Connection string 'BlogApplicationContext' not found.")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole> ()
                .AddEntityFrameworkStores<BlogApplicationContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/AccessDenied";
            });

            var app = builder.Build();

            DataSeeding();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseNotyf();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseStatusCodePagesWithRedirects("/StatusCodeError/{0}");

            app.Run();

            void DataSeeding()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var DbInitialize = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                    DbInitialize.Initialize();
                }
            }
        }
    }
}
