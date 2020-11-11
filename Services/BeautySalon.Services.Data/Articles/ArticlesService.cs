namespace BeautySalon.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Cloudinary;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class ArticlesService : IArticlesService
    {
        private readonly IRepository<Article> articlesRepository;
        private readonly IRepository<ClientArticleLike> clientArticleLikesRepository;
        private readonly ICloudinaryService cloudinaryService;

        public ArticlesService(IRepository<Article> articlesRepository, IRepository<ClientArticleLike> clientArticleLikesRepository, ICloudinaryService cloudinaryService)
        {
            this.articlesRepository = articlesRepository;
            this.clientArticleLikesRepository = clientArticleLikesRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<bool> CheckFavouriteArticlesAsync(string id, string userId)
        {
            var isFavourite = await this.clientArticleLikesRepository
                .All()
                .AnyAsync(ca => ca.ArticleId == id && ca.ClientId == userId);

            return isFavourite;
        }

        public async Task<string> CreateAsync(string title, string content, string categoryId, IFormFile picture, string stylistId)
        {
            string pictureAsUrl = await this.GetPictureAsUrl(title, picture);

            var article = new Article()
            {
                Title = title,
                Content = content,
                CategoryId = categoryId,
                Picture = pictureAsUrl,
                StylistId = stylistId,
            };

            await this.articlesRepository.AddAsync(article);
            await this.articlesRepository.SaveChangesAsync();

            return article.Id;
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

        public async Task<T> GetDataForUpdateAsync<T>(string id)
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

        public async Task<string> GetPictureUrlAsync(string id)
        {
            var pictureUrl = await this.articlesRepository
                .All()
                .Where(a => a.Id == id)
                .Select(a => a.Picture)
                .FirstOrDefaultAsync();

            return pictureUrl;
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

        public async Task UpdateAsync(string title, string content, string categoryId, IFormFile newPicture, string id)
        {
            var article = await this.articlesRepository
                .All()
                .FirstOrDefaultAsync(a => a.Id == id);

            article.Title = title;
            article.Content = content;
            article.CategoryId = categoryId;

            if (newPicture != null)
            {
                string pictureAsUrl = await this.GetPictureAsUrl(title, newPicture);
                article.Picture = pictureAsUrl;
            }

            this.articlesRepository.Update(article);
            await this.articlesRepository.SaveChangesAsync();
        }

        private async Task<string> GetPictureAsUrl(string title, IFormFile picture)
        {
            return await this.cloudinaryService.UploudAsync(picture, title);
        }
    }
}
