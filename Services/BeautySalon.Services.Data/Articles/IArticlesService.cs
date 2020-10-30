namespace BeautySalon.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IArticlesService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<T> GetArticleDetailsAsync<T>(string id);

        Task<bool> CheckFavouriteArticlesAsync(string id, string userId);

        Task<bool> LikeArticleAsync(string articleId, string userId);

        Task<int> GetLikesCountAsync(string articleId);
    }
}
