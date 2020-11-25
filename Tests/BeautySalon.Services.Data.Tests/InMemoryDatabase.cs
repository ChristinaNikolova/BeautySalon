namespace BeautySalon.Services.Data.Tests
{
    using System;

    using BeautySalon.Data;
    using Microsoft.EntityFrameworkCore;

    public class InMemoryDatabase
    {
        public static ApplicationDbContext Get()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(dbOptions);
        }
    }
}
