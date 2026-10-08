using CNHSworkloadAndClassSchedulingSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CNHSworkloadAndClassSchedulingSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<Data.ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<Models.Domain.ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole>()
                .AddEntityFrameworkStores<Data.ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            await DbInitializer.InitializeAsync(app.Services);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
