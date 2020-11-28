namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Mapping;
    using Xunit;

    public class CategoriesServiceTests : BaseServiceTests
    {
        public CategoriesServiceTests()
        {
        }

        [Fact]
        public async Task CheckGettingAllAndAllAsSelectListItem()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Category>(db);
            var service = new CategoriesService(repository);

            var firstCategory = new Category() { Id = Guid.NewGuid().ToString() };
            var secondCategory = new Category() { Id = Guid.NewGuid().ToString() };
            var thirdCategory = new Category() { Id = Guid.NewGuid().ToString() };

            await db.Categories.AddAsync(firstCategory);
            await db.Categories.AddAsync(secondCategory);
            await db.Categories.AddAsync(thirdCategory);
            await db.SaveChangesAsync();

            var categoriesAsSelectedItems = await service.GetAllAsSelectListItemAsync();
            var allCategories = await service.GetAllAsync<TestCategoryModel>();

            Assert.Equal(3, categoriesAsSelectedItems.Count());
            Assert.Equal(3, allCategories.Count());
        }

        [Fact]
        public async Task CheckGettingCategoryByNameAndById()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Category>(db);
            var service = new CategoriesService(repository);

            var firstCategory = new Category()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test Category",
            };

            await db.Categories.AddAsync(firstCategory);
            await db.SaveChangesAsync();

            var categoryByName = await service.GetByNameAsync("Test Category");
            var categoryById = await service.GetByIdAsync(firstCategory.Id);

            var expectedCategory = repository
                .All()
                .FirstOrDefault(c => c.Id == firstCategory.Id);

            Assert.True(categoryByName.Equals(expectedCategory));
            Assert.True(categoryById.Equals(expectedCategory));
        }

        public class TestCategoryModel : IMapFrom<Category>
        {
            public string Id { get; set; }
        }
    }
}
