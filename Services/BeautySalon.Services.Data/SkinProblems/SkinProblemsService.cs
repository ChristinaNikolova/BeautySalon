namespace BeautySalon.Services.Data.SkinProblems
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class SkinProblemsService : ISkinProblemsService
    {
        private readonly IRepository<SkinProblem> skinProblemsRepository;

        public SkinProblemsService(IRepository<SkinProblem> skinProblemsRepository)
        {
            this.skinProblemsRepository = skinProblemsRepository;
        }

        public async Task<string> CreateAsync(string name, string description)
        {
            var skinProblem = new SkinProblem()
            {
                Name = name,
                Description = description,
            };

            await this.skinProblemsRepository.AddAsync(skinProblem);
            await this.skinProblemsRepository.SaveChangesAsync();

            return skinProblem.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var skinProblem = await this.GetByIdAsync(id);

            skinProblem.IsDeleted = true;

            this.skinProblemsRepository.Update(skinProblem);
            await this.skinProblemsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string name, string description, string id)
        {
            var skinProblem = await this.GetByIdAsync(id);

            skinProblem.Name = name;
            skinProblem.Description = description;

            this.skinProblemsRepository.Update(skinProblem);
            await this.skinProblemsRepository.SaveChangesAsync();
        }

        public async Task<IList<SelectListItem>> GetAllAsSelectListItemAsync()
        {
            var skinProblems = await this.skinProblemsRepository
                .All()
                .Select(c => new SelectListItem()
                {
                    Value = c.Id,
                    Text = c.Name,
                })
                .ToListAsync();

            return skinProblems;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var skinProblems = await this.skinProblemsRepository
                .All()
                .OrderBy(sp => sp.Name)
                .To<T>()
                .ToListAsync();

            return skinProblems;
        }

        public async Task<SkinProblem> GetByIdAsync(string id)
        {
            return await this.skinProblemsRepository
                            .All()
                            .FirstOrDefaultAsync(sp => sp.Id == id);
        }
    }
}
