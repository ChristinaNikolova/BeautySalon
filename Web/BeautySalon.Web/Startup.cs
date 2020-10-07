namespace BeautySalon.Web
{
    using System.Reflection;

    using BeautySalon.Data;
    using BeautySalon.Data.Common;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Data.Seeding;
    using BeautySalon.Services.Cloudinary;
    using BeautySalon.Services.Data.Settings;
    using BeautySalon.Services.Data.Stylists;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Services.Mapping;
    using BeautySalon.Services.Messaging;
    using BeautySalon.Web.Hubs;
    using BeautySalon.Web.MLModels;
    using BeautySalon.Web.ViewModels;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.ML;
    using SendGrid;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IStylistsService, StylistsService>();

            // Add Facebook Authentication
            services.AddAuthentication()
               .AddFacebook(option =>
               {
                   option.AppId = this.configuration["Facebook:AppId"];
                   option.AppSecret = this.configuration["Facebook:AppSecret"];
                   option.AccessDeniedPath = "/AccessDeniedPathInfo";
               });

            // Add Google Authentication
            services.AddAuthentication()
               .AddGoogle(option =>
               {
                   option.ClientId = this.configuration["Google:ClientId"];
                   option.ClientSecret = this.configuration["Google:ClientSecret"];
                   option.ClaimActions.MapJsonKey("picture", "picture");
               });

            // Add Cloudinary
            var cloudinary = new Cloudinary(new Account()
            {
                Cloud = this.configuration["Cloudinary:AppName"],
                ApiKey = this.configuration["Cloudinary:AppKey"],
                ApiSecret = this.configuration["Cloudinary:AppSecret"],
            });

            services.AddSingleton(cloudinary);

            // Add SendGrid
            var sendGrid = new SendGridClient(this.configuration["SendGrid:ApiKey"]);
            services.AddSingleton(sendGrid);

            // Add SignalR
            services.AddSignalR();

            // Add ML
            services.AddPredictionEnginePool<SkinTypeModelInput, SkinTypeModelOutput>()
           .FromFile("MLModels/MLModel.zip");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapHub<ChatHub>("/chat");
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
