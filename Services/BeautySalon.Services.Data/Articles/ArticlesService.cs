namespace BeautySalon.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
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

        public async Task<IEnumerable<T>> GetAllAsync<T>(int take, int skip)
        {
            var articles = await this.articlesRepository
                .All()
                .OrderByDescending(a => a.CreatedOn)
                .ThenBy(a => a.Title)
                .Skip(skip)
                .Take(take)
                .To<T>()
                .ToListAsync();

            return articles;
        }

        public async Task<IEnumerable<T>> GetAllForStylistAsync<T>(string stylistId)
        {
            var articles = await this.articlesRepository
                .All()
                .Where(a => a.StylistId == stylistId)
                .OrderByDescending(a => a.CreatedOn)
                .OrderByDescending(a => a.Likes.Count())
                .OrderByDescending(a => a.Comments.Count())
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

        public async Task<int> GetLikesCountAsync(string articleId)
        {
            var count = await this.clientArticleLikesRepository
                .All()
                .Where(ac => ac.ArticleId == articleId)
                .CountAsync();

            return count;
        }

        public async Task<IEnumerable<T>> GetRecentArticlesAsync<T>()
        {
            var articles = await this.articlesRepository
                .All()
                .OrderByDescending(a => a.CreatedOn)
                .ThenBy(a => a.Title)
                .Take(GlobalConstants.RecentArticlesCount)
                .To<T>()
                .ToListAsync();

            return articles;
        }

        public async Task<int> GetTotalCountArticlesAsync()
        {
            var articlesCount = await this.articlesRepository
                .All()
                .CountAsync();

            return articlesCount;
        }

        public async Task<bool> LikeArticleAsync(string articleId, string userId)
        {
            var isFavourite = await this.CheckFavouriteArticlesAsync(articleId, userId);
            var isAdded = true;

            if (!isFavourite)
            {
                var clientArticleLike = new ClientArticleLike()
                {
                    ClientId = userId,
                    ArticleId = articleId,
                };

                await this.clientArticleLikesRepository.AddAsync(clientArticleLike);
            }
            else
            {
                var clientArticleLike = await this.clientArticleLikesRepository
                    .All()
                    .FirstOrDefaultAsync(ca => ca.ArticleId == articleId && ca.ClientId == userId);

                this.clientArticleLikesRepository.Delete(clientArticleLike);
                isAdded = false;
            }

            await this.clientArticleLikesRepository.SaveChangesAsync();

            return isAdded;
        }

        public async Task<IEnumerable<T>> SearchByAsync<T>(string categoryId)
        {
            var categories = await this.articlesRepository
                .All()
                .Where(a => a.CategoryId == categoryId)
                .OrderByDescending(a => a.CreatedOn)
                .ThenBy(a => a.Title)
                .To<T>()
                .ToListAsync();

            return categories;
        }
    }
}
