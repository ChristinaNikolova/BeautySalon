namespace BeautySalon.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IArticlesService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(int take, int skip);

        Task<T> GetArticleDetailsAsync<T>(string id);

        Task<bool> CheckFavouriteArticlesAsync(string id, string userId);

        Task<bool> LikeArticleAsync(string articleId, string userId);

        Task<int> GetLikesCountAsync(string articleId);

        Task<int> GetTotalCountArticlesAsync();

        Task<IEnumerable<T>> SearchByAsync<T>(string categoryId);

        Task<IEnumerable<T>> GetRecentArticlesAsync<T>();
    }
}
