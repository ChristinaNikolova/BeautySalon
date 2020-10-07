namespace BeautySalon.Services.Data.Stylists
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class StylistsService : IStylistsService
    {
        private readonly IRepository<ApplicationUser> stylistsRepository;
        private readonly IRepository<Category> categoriesRepository;
        private readonly IRepository<JobType> jobTypesRepository;

        public StylistsService(IRepository<ApplicationUser> stylistsepository, IRepository<Category> categoriesRepository, IRepository<JobType> jobTypesRepository)
        {
            this.stylistsRepository = stylistsepository;
            this.categoriesRepository = categoriesRepository;
            this.jobTypesRepository = jobTypesRepository;
        }

        public async Task<Category> GetStylistCategoryByNameAsync(string categoryName)
        {
            var category = await this.categoriesRepository
                .All()
                .FirstOrDefaultAsync(c => c.Name == categoryName);

            return category;
        }

        public async Task<Category> GetStylistCategoryByIdAsync(string categoryId)
        {
            var category = await this.categoriesRepository
                .All()
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            return category;
        }

        public async Task<JobType> GetStylistJobTypeByNameAsync(string jobTypeName)
        {
            var jobType = await this.jobTypesRepository
               .All()
               .FirstOrDefaultAsync(jt => jt.Name == jobTypeName);

            return jobType;
        }

        public async Task<JobType> GetStylistJobTypeByIdAsync(string jobTypeId)
        {
            var jobType = await this.jobTypesRepository
               .All()
               .FirstOrDefaultAsync(jt => jt.Id == jobTypeId);

            return jobType;
        }

        public async Task<ApplicationUser> UpdateStylistProfileAsync(string id, string categoryName, string jobTypeName, string descripion)
        {
            var stylist = await this.GetStylistByIdAsync(id);
            var category = await this.GetStylistCategoryByNameAsync(categoryName);
            var jobType = await this.GetStylistJobTypeByNameAsync(jobTypeName);

            stylist.CategoryId = category.Id;
            stylist.JobTypeId = jobType.Id;
            stylist.Description = descripion;

            await this.stylistsRepository.SaveChangesAsync();

            return stylist;
        }

        public async Task<ApplicationUser> GetStylistByIdAsync(string id)
        {
            var stylist = await this.stylistsRepository
                .All()
                .FirstOrDefaultAsync(s => s.Id == id);

            return stylist;
        }
    }
}
