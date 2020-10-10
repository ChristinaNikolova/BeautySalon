namespace BeautySalon.Services.Data.Brands
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IBrandsService
    {
        Task<string> CreateAsync(string name, string description, IFormFile logo);

        Task EditAsync(string name, IFormFile newLogo, string logo, string description, string id);

        Task DeleteAsync(string id);

        Task<Brand> GetByIdAsync(string id);

        Task<Brand> GetByNameAsync(string name);

        Task<IEnumerable<T>> GetAllAsync<T>();

        // Task<ExportDetailsBrand> GetBrandDetailsAsync(string brandId);

        // Task<InputEditBrand> GetSelectedBrandAsync(string brandId);

        // Task<IEnumerable<ExportBrandsByName>> GetAllBrandNamesAsync();
    }
}
