namespace BeautySalon.Data.Seeding.CustomSeeders
{
    using System;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedUserAsync(userManager, GlobalConstants.AdminEmail, GlobalConstants.AdminName, GlobalConstants.AdminName);
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, string userEmail, string firstName, string lastName)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == userEmail);

            if (user == null)
            {
                var admin = new ApplicationUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = firstName,
                    Email = userEmail,
                    EmailConfirmed = true,
                    PasswordHash = GlobalConstants.SystemPassword,
                    Picture = GlobalConstants.AdminPicture,
                };

                var creationResult = await userManager.CreateAsync(admin);
            }
        }
    }
}
