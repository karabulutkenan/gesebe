﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            // Kimlik doğrulama ve yetkilendirme
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Tablo}/{action=Index}/{id?}");
            });
        }
    }
}
