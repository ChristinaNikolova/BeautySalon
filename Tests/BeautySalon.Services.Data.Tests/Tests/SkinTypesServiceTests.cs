namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.SkinTypes;
    using BeautySalon.Services.Mapping;
    using Xunit;

    public class SkinTypesServiceTests : BaseServiceTests
    {
        public SkinTypesServiceTests()
        {
        }

        [Fact]
        public async Task CheckGettingAllAndGettingAllAsSelectListItem()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<SkinType>(db);
            var service = new SkinTypesService(repository);

            await AddSkinTypesToDbAsync(db);

            var skinTypesAsSelectListItems = await service.GetAllAsSelectListItemAsync();
            var skinTypes = await service.GetAllAsync<TestSkinTypeModel>();

            Assert.Equal(2, skinTypesAsSelectListItems.Count());
            Assert.Equal(2, skinTypes.Count());
        }

        [Fact]
        public async Task CheckGettingSkinTypeResult()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<SkinType>(db);
            var service = new SkinTypesService(repository);

            var firstSkinType = await AddSkinTypesToDbAsync(db);

            var skinType = await service.GetSkinTypeResultAsync<TestSkinTypeModel>(firstSkinType.Name);

            Assert.NotNull(skinType);
            Assert.Same(firstSkinType.Id, skinType.Id);
        }

        private static async Task<SkinType> AddSkinTypesToDbAsync(ApplicationDbContext db)
        {
            var firstSkinType = new SkinType() { Id = Guid.NewGuid().ToString(), Name = "Dry" };
            var secondSkinType = new SkinType() { Id = Guid.NewGuid().ToString(), Name = "Oily" };
            var thirdSkinType = new SkinType() { Id = Guid.NewGuid().ToString(), Name = "Sensitive" };

            await db.SkinTypes.AddAsync(firstSkinType);
            await db.SkinTypes.AddAsync(secondSkinType);
            await db.SkinTypes.AddAsync(thirdSkinType);
            await db.SaveChangesAsync();
            return firstSkinType;
        }

        public class TestSkinTypeModel : IMapFrom<SkinType>
        {
            public string Id { get; set; }
        }
    }
}
