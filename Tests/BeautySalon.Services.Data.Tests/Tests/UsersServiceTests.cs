namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class UsersServiceTests
    {
        public UsersServiceTests()
        {
            new MapperInitializationProfile();
        }

        [Fact]
        public async Task CheckGettingAllAndGettingAllAsSelectListItem()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var service = new UsersService(repository);

            //var firstSkinType = new SkinType() { Id = Guid.NewGuid().ToString() };
            //var secondSkinType = new SkinType() { Id = Guid.NewGuid().ToString() };
            //var thirdSkinType = new SkinType() { Id = Guid.NewGuid().ToString(), Name = "Sensitive" };

            //await db.SkinTypes.AddAsync(firstSkinType);
            //await db.SkinTypes.AddAsync(secondSkinType);
            //await db.SkinTypes.AddAsync(thirdSkinType);
            //await db.SaveChangesAsync();

            //var skinTypesAsSelectListItems = await service.GetAllAsSelectListItemAsync();
            //var skinTypes = await service.GetAllAsync<TestSkinTypeModel>();

            //Assert.Equal(2, skinTypesAsSelectListItems.Count());
            //Assert.Equal(2, skinTypes.Count());
        }

        private static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            return db;
        }

        public class TestUserModel : IMapFrom<ApplicationUser>
        {
            public string Id { get; set; }
        }
    }
}
