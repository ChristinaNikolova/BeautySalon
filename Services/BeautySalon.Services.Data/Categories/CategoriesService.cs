namespace BeautySalon.Services.Data.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<Category> categoriesRepository;

        public CategoriesService(IRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllAsSelectListItemAsync()
        {
            var categories = await this.categoriesRepository
                .All()
                .Select(c => new SelectListItem()
                {
                    Value = c.Id,
                    Text = c.Name,
                })
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var categories = await this.categoriesRepository
                .All()
                .OrderByDescending(c => c.Procedures.Count())
                .To<T>()
                .ToListAsync();

            return categories;
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            return await this.categoriesRepository
               .All()
               .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<Category> GetByIdAsync(string id)
        {
            return await this.categoriesRepository
               .All()
               .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<string> GetIdByNameAsync(string name)
        {
            var id = await this.categoriesRepository
                 .All()
                 .Where(c => c.Name == name)
                 .Select(c => c.Id)
                 .FirstOrDefaultAsync();

            return id;
        }
    }
}
