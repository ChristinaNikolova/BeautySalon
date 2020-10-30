namespace BeautySalon.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ArticlesService : IArticlesService
    {
        private readonly IRepository<Article> articlesRepository;
        private readonly IRepository<ClientArticleLike> clientArticleLikesRepository;

        public ArticlesService(IRepository<Article> articlesRepository, IRepository<ClientArticleLike> clientArticleLikesRepository)
        {
            this.articlesRepository = articlesRepository;
            this.clientArticleLikesRepository = clientArticleLikesRepository;
        }

        public async Task<bool> CheckFavouriteArticlesAsync(string id, string userId)
        {
            var isFavourite = await this.clientArticleLikesRepository
                .All()
                .AnyAsync(ca => ca.ArticleId == id && ca.ClientId == userId);

            return isFavourite;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var articles = await this.articlesRepository
                .All()
                .OrderByDescending(a => a.CreatedOn)
                .To<T>()
                .ToListAsync();

            return articles;
        }

        public async Task<T> GetArticleDetailsAsync<T>(string id)
        {
            var article = await this.articlesRepository
                .All()
                .Where(a => a.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return article;
        }
    }
}
