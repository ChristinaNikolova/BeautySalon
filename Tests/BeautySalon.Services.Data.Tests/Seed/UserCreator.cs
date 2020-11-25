using BeautySalon.Data.Models;
using BeautySalon.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeautySalon.Services.Data.Tests.Seed
{
    public class UserCreator
    {
        public static ApplicationUser Create(string firstName, string lastName, string userName, string email)
        {
            var user = new ApplicationUser()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                Gender = Gender.Female,
                Address = Guid.NewGuid().ToString(),
                PhoneNumber = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
            };

            return user;
        }
    }
}
