using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HeritageWebApplication.Models;
using HeritageWebApplication.Repository;
using HeritageWebApplication.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HeritageWebApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        public class Config
        {
            public static string ConnectionString { get; set; }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.Bind("Mailing", new MailConfig());

            //DB Connection
            Configuration.Bind("Project", new Config());
            services.AddDbContext<ApplicationDbContext>(x 
                => x.UseNpgsql(Config.ConnectionString), ServiceLifetime.Transient);

            //DB Data
            services.AddScoped<IRepository<Building>, BuildingRepository>();
            services.AddScoped<IRepository<HeritageObject>, HeritageObjectRepository>();
            services.AddScoped<IRepository<Comment>, CommentRepository>();
            services.AddScoped<IRepository<RenovationCompany>, RenovationRepository>();
            services.AddScoped<IDataManager, DataManager>();
            services.AddTransient<IImageService, ImageService>();
            
            
            services.AddIdentity<User, UserRole>(options =>
                    //services.AddIdentity<IdentityUser<int>, IdentityRole<int>>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<User, UserRole, ApplicationDbContext, int>>()
                .AddRoleStore<RoleStore<UserRole, ApplicationDbContext, int>>();


            //Other
            services.AddMvc();
            services.AddControllers();
            services.AddControllersWithViews();
            services.AddSignalR();
            services.AddSingleton<IInfoService, InfoService>();
            services.AddScoped<IEmailService>(t => new EmailService(
                MailConfig.Sender, MailConfig.SmtpServer, MailConfig.SmtpPort, MailConfig.Username, MailConfig.Password
            ));
            services.AddLogging(opt =>
            {
                opt.AddConsole();
                opt.AddFile(Path.Combine(Environment.WebRootPath, "logs/all.log"));
                opt.AddFile(Path.Combine(Environment.WebRootPath, "logs/error.log"), LogLevel.Error);
            });
            
            // Area for admin role
            services.AddAuthorization(x =>
            {
                x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
            });
            services.AddControllersWithViews(options =>
                {
                    options.Conventions.Add(new AdminAreaAuth("Admin", "AdminArea"));
                })
                .AddSessionStateTempDataProvider();

        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("/error/{0}");
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("admin", "{area:exists}/{controller=models}/{action=index}");
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }

}