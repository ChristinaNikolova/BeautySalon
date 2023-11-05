namespace BeautySalon.Services.Data.Comments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CommentsService : ICommentsService
    {
        private readonly IRepository<Comment> commentsRepository;

        public CommentsService(IRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task CreateAsync(string content, string articleId, string userId)
        {
            var comment = new Comment()
            {
                Content = content,
                ClientId = userId,
                ArticleId = articleId,
                CreatedOn = DateTime.UtcNow,
            };

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var comment = await this.commentsRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == id);

            comment.IsDeleted = true;

            this.commentsRepository.Update(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string articleId)
        {
            var comments = await this.commentsRepository
                .All()
                .Where(c => c.ArticleId == articleId)
                .OrderByDescending(c => c.CreatedOn)
                .To<T>()
                .ToListAsync();

            return comments;
        }

        public async Task<IEnumerable<T>> GetAllFromPreviousDayAsync<T>()
        {
            var previousDay = DateTime.UtcNow.Date.AddDays(-1);

            var comments = await this.commentsRepository
                .All()
                .Where(c => c.CreatedOn.Date == previousDay)
                .OrderBy(c => c.CreatedOn)
                .ThenBy(c => c.Article.Title)
                .ThenBy(c => c.Client.UserName)
                .To<T>()
                .ToListAsync();

            return comments;
        }
    }
}
