namespace BeautySalon.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IArticlesService
    {
        Task<string> CreateAsync(string title, string content, string categoryId, IFormFile picture, string stylistId);

        Task<T> GetDataForUpdateAsync<T>(string id);

        Task UpdateAsync(string title, string content, string categoryId, IFormFile newPicture, string id);

        Task<string> GetPictureUrlAsync(string id);

        Task DeleteAsync(string id);

        Task<IEnumerable<T>> GetAllAsync<T>(int take, int skip);

        Task<T> GetArticleDetailsAsync<T>(string id);

        Task<bool> CheckFavouriteArticlesAsync(string id, string userId);

        Task<bool> LikeArticleAsync(string articleId, string userId);

        Task<int> GetLikesCountAsync(string articleId);

        Task<int> GetTotalCountArticlesAsync();

        Task<IEnumerable<T>> SearchByAsync<T>(string categoryId);

        Task<IEnumerable<T>> GetRecentArticlesAsync<T>();

        Task<IEnumerable<T>> GetAllForStylistAsync<T>(string stylistId);

        Task<IEnumerable<T>> GetUsersFavouriteArticlesAsync<T>(string userId);
    }
}
