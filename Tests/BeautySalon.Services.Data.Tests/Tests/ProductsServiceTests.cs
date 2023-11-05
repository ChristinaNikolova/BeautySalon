namespace BeautySalon.Services.Data.Tests.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Cloudinary;
    using BeautySalon.Services.Data.Products;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class ProductsServiceTests : BaseServiceTests
    {
        private readonly Mock<IFormFile> mockPicture;

        private readonly Mock<IRepository<ClientProductLike>> clientProductLikesRepository;
        private readonly Mock<ICloudinaryService> cloudinaryService;

        public ProductsServiceTests()
        {
            this.clientProductLikesRepository = new Mock<IRepository<ClientProductLike>>();
            this.cloudinaryService = new Mock<ICloudinaryService>();
            this.mockPicture = new Mock<IFormFile>();
        }

        [Fact]
        public async Task CheckCreatingProductAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Product>(db);
            var service = new ProductsService(
                repository,
                this.clientProductLikesRepository.Object,
                this.cloudinaryService.Object);

            string productId = await this.PrepareProductAsync(service);

            Assert.NotNull(productId);
        }

        [Fact]
        public async Task CheckDeletingProductAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Product>(db);
            var service = new ProductsService(
                repository,
                this.clientProductLikesRepository.Object,
                this.cloudinaryService.Object);

            string productId = await this.PrepareProductAsync(service);

            await service.DeleteAsync(productId);

            Assert.Empty(repository.All());
        }

        [Fact]
        public async Task CheckUpdatingProductAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Product>(db);
            var service = new ProductsService(
                repository,
                this.clientProductLikesRepository.Object,
                this.cloudinaryService.Object);

            var brand = new Brand()
            {
                Id = "1",
                Name = "brandName",
            };

            var category = new Category()
            {
                Id = "1",
                Name = "categoryName",
            };

            var product = new Product()
            {
                Id = "1",
                Name = "productName",
                Description = "productDescription",
                Price = 12,
                BrandId = brand.Id,
                CategoryId = category.Id,
            };

            await db.Products.AddAsync(product);
            await db.Categories.AddAsync(category);
            await db.Brands.AddAsync(brand);
            await db.SaveChangesAsync();

            var picture = this.mockPicture.Object;

            await service.UpdateAsync(product.Id, "newProductName", product.Description, 13, picture, "2", product.CategoryId);

            var pictureAsUrl = await service.GetPictureUrlAsync(product.Id);

            Assert.Equal("newProductName", product.Name);
            Assert.Equal("productDescription", product.Description);
            Assert.Equal(13, product.Price);
            Assert.Equal(pictureAsUrl, product.Picture);
            Assert.Equal("2", product.BrandId);
            Assert.Equal("1", product.CategoryId);
        }

        [Fact]
        public async Task CheckGettingAllProductsAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Product>(db);
            var service = new ProductsService(
                repository,
                this.clientProductLikesRepository.Object,
                this.cloudinaryService.Object);

            var brand = new Brand()
            {
                Id = "1",
                Name = "brandName",
            };

            var firstProduct = new Product() { Id = "1", BrandId = brand.Id };
            var secondProduct = new Product() { Id = "2", BrandId = brand.Id };
            var thirdProduct = new Product() { Id = "3", BrandId = brand.Id };

            await db.Products.AddAsync(firstProduct);
            await db.Products.AddAsync(secondProduct);
            await db.Products.AddAsync(thirdProduct);
            await db.Brands.AddAsync(brand);
            await db.SaveChangesAsync();

            var products = await service.GetAllAdministrationAsync<TestProductModel>();

            Assert.Equal(3, products.Count());
        }

        [Fact]
        public async Task CheckGettingProductsDetailsAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Product>(db);
            var service = new ProductsService(
                repository,
                this.clientProductLikesRepository.Object,
                this.cloudinaryService.Object);

            var firstProduct = new Product() { Id = "1" };
            var secondProduct = new Product() { Id = "2" };

            await db.Products.AddAsync(firstProduct);
            await db.Products.AddAsync(secondProduct);
            await db.SaveChangesAsync();

            var productResult = await service.GetDetailsAsync<TestProductModel>(firstProduct.Id);

            Assert.Equal(firstProduct.Id, productResult.Id);
        }

        [Fact]
        public async Task CheckGettingProductIdByNameAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Product>(db);
            var service = new ProductsService(
                repository,
                this.clientProductLikesRepository.Object,
                this.cloudinaryService.Object);

            var firstProduct = new Product()
            {
                Id = "1",
                Name = "firstProductName",
            };

            var secondProduct = new Product()
            {
                Id = "2",
                Name = "secondProductName",
            };

            await db.Products.AddAsync(firstProduct);
            await db.Products.AddAsync(secondProduct);
            await db.SaveChangesAsync();

            var productId = await service.GetProductIdByNameAsync(firstProduct.Name);

            Assert.Equal(firstProduct.Id, productId);
        }

        [Fact]
        public async Task CheckFavouriteProductFalseCaseAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Product>(db);
            var clientProductLikesRepository = new EfRepository<ClientProductLike>(db);

            var service = new ProductsService(
                repository,
                clientProductLikesRepository,
                this.cloudinaryService.Object);

            var product = new Product()
            {
                Id = "1",
                Name = "firstProductName",
            };

            var client = new ApplicationUser()
            {
                Id = "1",
            };

            await db.Products.AddAsync(product);
            await db.Users.AddAsync(client);
            await db.SaveChangesAsync();

            var isFavourite = await service.CheckFavouriteProductsAsync(product.Id, client.Id);

            Assert.True(!isFavourite);
        }

        [Fact]
        public async Task CheckLikingProductAddAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Product>(db);
            var clientProductLikesRepository = new EfRepository<ClientProductLike>(db);

            var service = new ProductsService(
                repository,
                clientProductLikesRepository,
                this.cloudinaryService.Object);

            var product = new Product()
            {
                Id = "1",
                Name = "firstProductName",
            };

            var client = new ApplicationUser()
            {
                Id = "1",
            };

            await db.Products.AddAsync(product);
            await db.Users.AddAsync(client);
            await db.SaveChangesAsync();

            var isAddedTrueCase = await service.LikeProductAsync(product.Id, client.Id);
            var isAddedFalseCase = await service.LikeProductAsync(product.Id, client.Id);

            Assert.True(isAddedTrueCase);
            Assert.True(!isAddedFalseCase);
        }

        [Fact]
        public async Task CheckLikesProductsCountAndGetingLikesProductsAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Product>(db);
            var clientProductLikesRepository = new EfRepository<ClientProductLike>(db);

            var service = new ProductsService(
                repository,
                clientProductLikesRepository,
                this.cloudinaryService.Object);

            var firstProduct = new Product()
            {
                Id = "1",
                Name = "firstProductName",
            };

            var secondProduct = new Product()
            {
                Id = "2",
                Name = "secondProductName",
            };

            var client = new ApplicationUser()
            {
                Id = "1",
            };

            await db.Products.AddAsync(firstProduct);
            await db.Products.AddAsync(secondProduct);
            await db.Users.AddAsync(client);
            await db.SaveChangesAsync();

            await service.LikeProductAsync(firstProduct.Id, client.Id);

            var count = await service.GetLikesCountAsync(firstProduct.Id);
            var products = await service.GetUsersFavouriteProductsAsync<TestClientProductLikeModel>(client.Id);

            Assert.Equal(1, count);
            Assert.Single(products);
        }

        private async Task<string> PrepareProductAsync(ProductsService service)
        {
            var picture = this.mockPicture.Object;

            var productId = await service.CreateAsync("productName", "productDescription", 12, picture, "1", "1");

            return productId;
        }

        public class TestProductModel : IMapFrom<Product>
        {
            public string Id { get; set; }
        }

        public class TestClientProductLikeModel : IMapFrom<ClientProductLike>
        {
            public string Id { get; set; }
        }
    }
}
