namespace BeautySalon.Services.Data.Tests.Seed
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Data.Models.Enums;

    public class StylistCreator
    {
        public static ApplicationUser Create(string firstName, string lastName, string userName, string email)
        {
            var category = new Category()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Skin Care",
            };

            var jobType = new JobType()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Top Medical Cosmetician",
            };

            var stylist = new ApplicationUser()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                Gender = Gender.Female,
                Address = Guid.NewGuid().ToString(),
                PhoneNumber = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Category = category,
                JobType = jobType,
                EmailConfirmed = true,
            };

            return stylist;
        }
    }
}
