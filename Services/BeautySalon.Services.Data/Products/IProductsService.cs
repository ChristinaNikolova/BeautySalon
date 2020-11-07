namespace BeautySalon.Services.Data.Products
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IProductsService
    {
        Task<string> CreateAsync(string name, string description, decimal price, int quantity, IFormFile picture, string brandName, string categoryName);

        Task EditAsync(string name, string description, decimal price, int quantity, IFormFile newPicture, string brandName, string categoryName, string id);

        Task DeleteAsync(string id);

        Task<Product> GetByIdAsync(string id);

        Task<T> GetDetailsAsync<T>(string id);
    }
}
