namespace BeautySalon.Services.Data.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IProductsService
    {
        Task<string> CreateAsync(string name, string description, decimal price, IFormFile picture, string brandId, string categoryId);

        Task UpdateAsync(string id, string name, string description, decimal price, IFormFile newPicture, string brandId, string categoryId);

        Task DeleteAsync(string id);

        Task<Product> GetByIdAsync(string id);

        Task<T> GetDetailsAsync<T>(string id);

        Task<IEnumerable<T>> GetAllAdministrationAsync<T>();

        Task<T> GetProductDataForUpdateAsync<T>(string id);

        Task<string> GetPictureUrlAsync(string id);

        Task<string> GetProductIdByNameAsync(string productName);

        Task<bool> CheckFavouriteProductsAsync(string id, string userId);

        Task<bool> LikeProductAsync(string productId, string userId);

        Task<int> GetLikesCountAsync(string productId);
    }
}
