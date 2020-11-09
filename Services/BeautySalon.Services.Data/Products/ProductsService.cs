namespace BeautySalon.Services.Data.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
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

        public ProductsService(IRepository<Product> productsRepository, ICloudinaryService cloudinaryService)
        {
            this.productsRepository = productsRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<string> CreateAsync(string name, string description, decimal price, IFormFile picture, string brandId, string categoryId)
        {
            var pictureAsString = await this.GetPictureAsStringAsync(name, picture);

            var product = new Product()
            {
                Name = name,
                Description = description,
                Price = price,
                Quantity = GlobalConstants.DefaultProductQuantity,
                Picture = pictureAsString,
                CategoryId = categoryId,
                BrandId = brandId,
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

        public async Task UpdateAsync(string id, string name, string description, decimal price, IFormFile newPicture, string brandId, string categoryId)
        {
            var product = await this.GetByIdAsync(id);

            product.Name = name;
            product.Description = description;
            product.Price = price;
            product.CategoryId = categoryId;
            product.BrandId = brandId;

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

        public async Task<T> GetProductDataForUpdateAsync<T>(string id)
        {
            var product = await this.productsRepository
                 .All()
                 .Where(p => p.Id == id)
                 .To<T>()
                 .FirstOrDefaultAsync();

            return product;
        }

        public async Task<string> GetPictureUrlAsync(string id)
        {
            var pictureUrl = await this.productsRepository
                .All()
                .Where(p => p.Id == id)
                .Select(p => p.Picture)
                .FirstOrDefaultAsync();

            return pictureUrl;
        }

        private async Task<string> GetPictureAsStringAsync(string name, IFormFile picture)
        {
            return await this.cloudinaryService.UploudAsync(picture, name);
        }
    }
}
