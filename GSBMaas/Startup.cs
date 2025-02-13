using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Http;


namespace GSBMaas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication()
                .AddCookie("ModeratorAuth", options =>
                {
                    options.LoginPath = "/Moderator/Giris";
                    options.AccessDeniedPath = "/Moderator/Giris";
                })
                .AddCookie("YoneticiAuth", options =>
                {
                    options.LoginPath = "/Yonetici/Giris";
                    options.AccessDeniedPath = "/Yonetici/Giris";
                });

            // ✅ **Session ve Cache Servisleri Eklendi**
            services.AddDistributedMemoryCache(); // Session için bellek içi cache
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session süresi 30 dakika
                options.Cookie.HttpOnly = true;  // Tarayıcı tarafında erişimi sınırla
                options.Cookie.IsEssential = true; // GDPR uyumluluğu
                options.Cookie.SameSite = SameSiteMode.Strict; // Cross-site saldırılara karşı önlem
            });


            // Yetkilendirme servisini ekleyin
            services.AddAuthorization();

            // MVC servislerini ekleyin
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseSession();  // ✅ **Session Middleware'i Doğru Yerde!**

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
