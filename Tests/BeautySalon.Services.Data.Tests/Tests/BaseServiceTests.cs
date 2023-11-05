namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;

    using BeautySalon.Data;
    using Microsoft.EntityFrameworkCore;

    public class BaseServiceTests
    {
        public BaseServiceTests()
        {
            new MapperInitializationProfile();
        }

        public static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);

            return db;
        }
    }
}
