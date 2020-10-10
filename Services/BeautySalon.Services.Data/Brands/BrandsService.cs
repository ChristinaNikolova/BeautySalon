namespace BeautySalon.Services.Data.Brands
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Cloudinary;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class BrandsService : IBrandsService
    {
        private readonly IRepository<Brand> brandsRepository;
        private readonly ICloudinaryService cloudinaryService;

        public BrandsService(IRepository<Brand> brandsRepository, ICloudinaryService cloudinaryService)
        {
            this.brandsRepository = brandsRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<string> CreateAsync(string name, string description, IFormFile logo)
        {
            string logoAsString = await this.GetLogoAsStringAsync(name, logo);

            var brand = new Brand()
            {
                Name = name,
                Description = description,
                Logo = logoAsString,
            };

            await this.brandsRepository.AddAsync(brand);
            await this.brandsRepository.SaveChangesAsync();

            return brand.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var brand = await this.GetByIdAsync(id);

            brand.IsDeleted = true;

            this.brandsRepository.Update(brand);
            await this.brandsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string name, IFormFile newLogo, string logo, string description, string id)
        {
            var brand = await this.GetByIdAsync(id);

            if (newLogo != null)
            {
                string logoAsString = await this.GetLogoAsStringAsync(name, newLogo);
                brand.Logo = logoAsString;
            }

            brand.Name = name;
            brand.Description = description;

            this.brandsRepository.Update(brand);
            await this.brandsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var brands = await this.brandsRepository
                .All()
                .To<T>()
                .ToListAsync();

            return brands;
        }

        public async Task<Brand> GetByIdAsync(string id)
        {
            return await this.brandsRepository
                            .All()
                            .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Brand> GetByNameAsync(string name)
        {
            return await this.brandsRepository
                            .All()
                            .FirstOrDefaultAsync(b => b.Name == name);
        }

        private async Task<string> GetLogoAsStringAsync(string name, IFormFile logo)
        {
            return await this.cloudinaryService.UploudAsync(logo, name);
        }
    }
}
