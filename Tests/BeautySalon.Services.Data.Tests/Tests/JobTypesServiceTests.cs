namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.JobTypes;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class JobTypesServiceTests
    {
        public JobTypesServiceTests()
        {
        }

        [Fact]
        public async Task CheckGettingAllAsSelectListItem()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<JobType>(db);
            var service = new JobTypesService(repository);

            var firstJobType = new JobType() { Id = Guid.NewGuid().ToString() };
            var secondJobType = new JobType() { Id = Guid.NewGuid().ToString() };
            var thirdJobType = new JobType() { Id = Guid.NewGuid().ToString() };

            await db.JobTypes.AddAsync(firstJobType);
            await db.JobTypes.AddAsync(secondJobType);
            await db.JobTypes.AddAsync(thirdJobType);
            await db.SaveChangesAsync();

            var jobTypes = await service.GetAllAsSelectListItemAsync();

            Assert.Equal(3, jobTypes.Count());
        }

        private static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            return db;
        }
    }
}
