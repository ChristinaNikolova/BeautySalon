namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.SkinProblems;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class SkinProblemsServiceTests
    {
        public SkinProblemsServiceTests()
        {
            new MapperInitializationProfile();
        }

        [Fact]
        public async Task CheckGettingAllAndGettingAllAsSelectListItem()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<SkinProblem>(db);
            var service = new SkinProblemsService(repository);

            var firstSkinProblem = new SkinProblem() { Id = Guid.NewGuid().ToString() };
            var secondSkinProblem = new SkinProblem() { Id = Guid.NewGuid().ToString() };
            var thirdSkinProblem = new SkinProblem() { Id = Guid.NewGuid().ToString() };

            await db.SkinProblems.AddAsync(firstSkinProblem);
            await db.SkinProblems.AddAsync(secondSkinProblem);
            await db.SkinProblems.AddAsync(thirdSkinProblem);
            await db.SaveChangesAsync();

            var skinProblemsAsSelectListItems = await service.GetAllAsSelectListItemAsync();
            var skinProblems = await service.GetAllAsync<TestSkinProblemModel>();

            Assert.Equal(3, skinProblemsAsSelectListItems.Count());
            Assert.Equal(3, skinProblems.Count());
        }

        private static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            return db;
        }

        public class TestSkinProblemModel : IMapFrom<SkinProblem>
        {
            public string Id { get; set; }
        }
    }
}
