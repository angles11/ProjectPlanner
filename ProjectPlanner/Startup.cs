
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ProjectPlanner.Data;
using ProjectPlanner.Email;
using ProjectPlanner.Models;
using System;

namespace ProjectPlanner
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
            services.AddControllersWithViews();

            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    //IConfigurationSection googleAuthNSection =
                    //        Configuration.GetSection("Authentication:Google");
                    //options.ClientId = googleAuthNSection["ClientId"];
                    //options.ClientSecret = googleAuthNSection["ClientSecret"];

                    googleOptions.ClientId = "908480960051-63eh3lesef8ef8337v28saouo3gibf1u.apps.googleusercontent.com";
                    googleOptions.ClientSecret = "_mvX_gNiBJoqvqahPPTQPe6n";
                })
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.ClientId = "1375849815928084";
                    facebookOptions.ClientSecret = "67726a9381b1a4fce20456b3328096ed";

                })
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = "d89a03bf-8d69-43b3-aa97-8ac6830ad54a";
                    microsoftOptions.ClientSecret = "d.:BCz8bPqQ.-fye5G1n0kp_38MYTxdx";
                });

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(3));            

            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddSingleton<IEmailSender, EmailSender>();
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                name: "todos",
                pattern: "projects/{id:int}/todos",
                defaults: new { controller = "Todos", action = "Index" });
            });
        }
    }
}
