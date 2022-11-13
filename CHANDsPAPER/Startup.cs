using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace CHANDsPAPER
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            //This is for increasing size of the file
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 1000000000;
            });
            //services.AddDistributedMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "B1Payment";
                options.LoginPath = "/Login/Index";
                 options.AccessDeniedPath = new PathString("/Auth");
                 //options.LoginPath = "/Login/Index";
                 //options.Cookie.Name = "TriedCookie";
             });
            services.AddSession(options =>
            {
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
            });

            services.AddMvc();
            services.AddDistributedMemoryCache();
            
            services.AddControllersWithViews();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // services.AddResponseCaching();
            //services.AddIdentity<IdentityUser, IdentityRole>(config =>
            //{
            //    config.SignIn.RequireConfirmedEmail = true;
            //});




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

           // app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseCookiePolicy();
           // app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
