using AspNetCoreHero.ToastNotification;
using DataLab.DataManager;
using DataLab.IServices;
using DataLab.Models;
using DataLab.Repositories;
using DataLab.SeedHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddDbContextPool<DataLabDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DataLab-DatabaseConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DataLabDbContext>();
            services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

            services.AddScoped<ICustomerService, CustomerRepository>();
            services.AddScoped<IAuthUserService, AuthUserRepository>();
            services.AddScoped<ISensorService, SensorRepository>();
            services.AddScoped<IDataService, DataRepository>();
            services.AddScoped<IAccessService, AccessRepository>();
            services.AddScoped<ISeedInitializer, SeedInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedInitializer _seedInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Errors/DashBordErrorStatusCodes", "?code={0}");
                app.UseExceptionHandler("/Errors/Error");

                
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            _seedInitializer.ExecuteSeed();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
