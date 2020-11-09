namespace BeautySalon.Services.Data.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Cloudinary;
    using BeautySalon.Services.Data.Brands;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class ProductsService : IProductsService
    {
        private readonly IRepository<Product> productsRepository;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IBrandsService brandsService;
        private readonly ICategoriesService categoriesService;

        public ProductsService(IRepository<Product> productsRepository, ICloudinaryService cloudinaryService, IBrandsService brandsService, ICategoriesService categoriesService)
        {
            this.productsRepository = productsRepository;
            this.cloudinaryService = cloudinaryService;
            this.brandsService = brandsService;
            this.categoriesService = categoriesService;
        }

        public async Task<string> CreateAsync(string name, string description, decimal price, int quantity, IFormFile picture, string brandName, string categoryName)
        {
            var pictureAsString = await this.GetPictureAsStringAsync(name, picture);
            var category = await this.categoriesService.GetByNameAsync(categoryName);
            var brand = await this.brandsService.GetByNameAsync(brandName);

            var product = new Product()
            {
                Name = name,
                Description = description,
                Price = price,
                Quantity = quantity,
                Picture = pictureAsString,
                CategoryId = category.Id,
                BrandId = brand.Id,
            };

            await this.productsRepository.AddAsync(product);
            await this.productsRepository.SaveChangesAsync();

            return product.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var product = await this.GetByIdAsync(id);

            product.IsDeleted = true;

            this.productsRepository.Update(product);
            await this.productsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string name, string description, decimal price, int quantity, IFormFile newPicture, string brandName, string categoryName, string id)
        {
            var product = await this.GetByIdAsync(id);
            var category = await this.categoriesService.GetByNameAsync(categoryName);
            var brand = await this.brandsService.GetByNameAsync(brandName);

            product.Name = name;
            product.Description = description;
            product.Price = price;
            product.Quantity = quantity;
            product.CategoryId = category.Id;
            product.BrandId = brand.Id;

            if (newPicture != null)
            {
                var pictureAsString = await this.GetPictureAsStringAsync(name, newPicture);
                product.Picture = pictureAsString;
            }

            this.productsRepository.Update(product);
            await this.productsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAdministrationAsync<T>()
        {
            var products = await this.productsRepository
                .All()
                .OrderBy(p => p.Brand.Name)
                .ThenBy(p => p.Name)
                .ThenBy(p => p.Price)
                .To<T>()
                .ToListAsync();

            return products;
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await this.productsRepository
                            .All()
                            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<T> GetDetailsAsync<T>(string id)
        {
            var product = await this.productsRepository
                .All()
                .Where(p => p.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return product;
        }

        private async Task<string> GetPictureAsStringAsync(string name, IFormFile picture)
        {
            return await this.cloudinaryService.UploudAsync(picture, name);
        }
    }
}
