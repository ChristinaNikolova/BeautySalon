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

        public ArticlesService(IRepository<Article> articlesRepository)
        {
            this.articlesRepository = articlesRepository;
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
    }
}
