namespace BeautySalon.Web
{
    using System;
    using System.Reflection;
    using BeautySalon.Common;
    using BeautySalon.CronJobs;
    using BeautySalon.Data;
    using BeautySalon.Data.Common;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Data.Seeding;
    using BeautySalon.Services.Cloudinary;
    using BeautySalon.Services.Data.Answers;
    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Services.Data.Articles;
    using BeautySalon.Services.Data.Brands;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.ChatMessages;
    using BeautySalon.Services.Data.Comments;
    using BeautySalon.Services.Data.JobTypes;
    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Services.Data.Products;
    using BeautySalon.Services.Data.Questions;
    using BeautySalon.Services.Data.Quiz;
    using BeautySalon.Services.Data.Reviews;
    using BeautySalon.Services.Data.Settings;
    using BeautySalon.Services.Data.SkinProblems;
    using BeautySalon.Services.Data.SkinTypes;
    using BeautySalon.Services.Data.Stylists;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Services.Mapping;
    using BeautySalon.Services.Messaging;
    using BeautySalon.Web.Hubs;
    using BeautySalon.Web.SecurityModels;
    using BeautySalon.Web.ViewModels;
    using BeautySalon.Web.ViewModels.MLModels;
    using CloudinaryDotNet;
    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.SqlServer;
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
            // Add Hangfire
            services.AddHangfire(
               config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                   .UseSimpleAssemblyNameTypeSerializer()
                   .UseRecommendedSerializerSettings()
                   .UseSqlServerStorage(
                       this.configuration.GetConnectionString("DefaultConnection"),
                       new SqlServerStorageOptions
                       {
                           CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                           SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                           QueuePollInterval = TimeSpan.Zero,
                           UseRecommendedIsolationLevel = true,
                           UsePageLocksOnDequeue = true,
                           DisableGlobalLocks = true,
                       }));

            // Add Db
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
            services.AddTransient<ICloudinaryService, CloudinaryService>();

            services.AddTransient<IBrandsService, BrandsService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IChatsService, ChatsService>();
            services.AddTransient<IJobTypesService, JobTypesService>();
            services.AddTransient<IProceduresService, ProceduresService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ISkinProblemsService, SkinProblemsService>();
            services.AddTransient<ISkinTypesService, SkinTypesService>();
            services.AddTransient<IStylistsService, StylistsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IQuestionsService, QuestionsService>();
            services.AddTransient<IArticlesService, ArticlesService>();
            services.AddTransient<ICommentsService, CommentsService>();
            services.AddTransient<IAppointmentsService, AppointmentsService>();
            services.AddTransient<IQuizService, QuizService>();
            services.AddTransient<IAnswersService, AnswersService>();
            services.AddTransient<IReviewsService, ReviewsService>();

            // Add Antiforgery
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

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

            // Add reCAPTCHA
            services.Configure<GoogleReCAPTCHA>(this.configuration.GetSection("GoogleReCAPTCHA"));

            // Add ML
            services.AddPredictionEnginePool<SkinTypeModelInput, SkinTypeModelOutput>()
           .FromFile("MLModels/MLModel.zip");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IRecurringJobManager recurringJobManager)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
                this.SeedHangfireJobs(recurringJobManager, dbContext);
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

            // Add Hangfire Dashboard
            app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 2 });
            app.UseHangfireDashboard(
                "/Administration/HangFire",
                new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });

            // Routes
            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapHub<ChatHub>("/chat");
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("procedureCategory", "{controller=Procedures}/{action=GetProceduresByCategory}/{id?}/{currentPage?}");
                        endpoints.MapRazorPages();
                    });
        }

        private void SeedHangfireJobs(IRecurringJobManager recurringJobManager, ApplicationDbContext dbContext)
        {
            //Have to change - weekly!!!
            //add requirement to delete
            recurringJobManager
                .AddOrUpdate<DeleteChatMessages>(
                "DeleteChatMessages", x => x.DeleteAsync(), Cron.Weekly);

            //recurringJobManager
            //   .AddOrUpdate<DeleteOldAppointments>(
            //   "DeleteOldAppointments", x => x.DeleteOldAppointments(), Cron.Minutely);
        }

        private class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();
                return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
