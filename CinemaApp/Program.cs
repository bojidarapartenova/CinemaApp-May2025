using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
namespace CinemaApp.Web
{
    using CinemaApp.Data;
    using CinemaApp.Services.Core;
    using CinemaApp.Services.Core.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<CinemaAppDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services
                            .AddDefaultIdentity<IdentityUser>(options =>
                            {
                                options.SignIn.RequireConfirmedEmail = false;
                                options.SignIn.RequireConfirmedAccount = false;
                                options.SignIn.RequireConfirmedPhoneNumber = false;

                                options.Password.RequiredLength = 3;
                                options.Password.RequireNonAlphanumeric = false;
                                options.Password.RequireDigit = false;
                                options.Password.RequireLowercase = false;
                                options.Password.RequireUppercase = false;
                                options.Password.RequiredUniqueChars = 0;
                            })
                            .AddEntityFrameworkStores<CinemaAppDbContext>();

            builder.Services.AddScoped<IMovieService, MovieService>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
