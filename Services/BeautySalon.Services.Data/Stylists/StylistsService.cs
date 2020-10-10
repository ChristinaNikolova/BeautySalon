namespace BeautySalon.Services.Data.Stylists
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.JobTypes;
    using Microsoft.EntityFrameworkCore;

    public class StylistsService : IStylistsService
    {
        private readonly IRepository<ApplicationUser> stylistsRepository;
        private readonly ICategoriesService categoriesService;
        private readonly IJobTypesService jobTypesService;

        public StylistsService(IRepository<ApplicationUser> stylistsepository, ICategoriesService categoriesService, IJobTypesService jobTypesService)
        {
            this.stylistsRepository = stylistsepository;
            this.categoriesService = categoriesService;
            this.jobTypesService = jobTypesService;
        }

        public async Task<ApplicationUser> UpdateStylistProfileAsync(string id, string categoryName, string jobTypeName, string descripion)
        {
            var stylist = await this.GetByIdAsync(id);
            var category = await this.categoriesService.GetByNameAsync(categoryName);
            var jobType = await this.jobTypesService.GetByNameAsync(jobTypeName);

            stylist.CategoryId = category.Id;
            stylist.JobTypeId = jobType.Id;
            stylist.Description = descripion;

            await this.stylistsRepository.SaveChangesAsync();

            return stylist;
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            var stylist = await this.stylistsRepository
                .All()
                .FirstOrDefaultAsync(s => s.Id == id);

            return stylist;
        }

        public async Task DeleteAsync(string id)
        {
            var stylist = await this.GetByIdAsync(id);

            stylist.IsDeleted = true;

            this.stylistsRepository.Update(stylist);
            await this.stylistsRepository.SaveChangesAsync();
        }
    }
}
