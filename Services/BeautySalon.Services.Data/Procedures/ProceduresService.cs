namespace BeautySalon.Services.Data.Procedures
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.SkinTypes;
    using Microsoft.EntityFrameworkCore;

    public class ProceduresService : IProceduresService
    {
        // TODO: add products to procedure by create/edit

        // TODO: add SkinProblems to procedure by create/edit
        private readonly IRepository<Procedure> proceduresRepository;
        private readonly ICategoriesService categoriesService;
        private readonly ISkinTypesService skinTypesService;

        public ProceduresService(IRepository<Procedure> proceduresRepository, ICategoriesService categoriesService, ISkinTypesService skinTypesService)
        {
            this.proceduresRepository = proceduresRepository;
            this.categoriesService = categoriesService;
            this.skinTypesService = skinTypesService;
        }

        public async Task<string> CreateAsync(string name, string description, decimal price, string categoryName, string skinTypeName)
        {
            var category = await this.categoriesService.GetByNameAsync(categoryName);
            var skinType = await this.skinTypesService.GetByNameAsync(skinTypeName);

            var procedure = new Procedure()
            {
                Name = name,
                Description = description,
                Price = price,
                CategoryId = category.Id,
                SkinTypeId = skinType.Id,
            };

            await this.proceduresRepository.AddAsync(procedure);
            await this.proceduresRepository.SaveChangesAsync();

            return procedure.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var procedure = await this.GetByIdAsync(id);

            procedure.IsDeleted = true;

            this.proceduresRepository.Update(procedure);
            await this.proceduresRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string name, string description, decimal price, string categoryName, string skinTypeName, string id)
        {
            var procedure = await this.GetByIdAsync(id);
            var category = await this.categoriesService.GetByNameAsync(categoryName);
            var skinType = await this.skinTypesService.GetByNameAsync(skinTypeName);

            procedure.Name = name;
            procedure.Description = description;
            procedure.Price = price;
            procedure.CategoryId = category.Id;
            procedure.SkinTypeId = skinType.Id;

            this.proceduresRepository.Update(procedure);
            await this.proceduresRepository.SaveChangesAsync();
        }

        public async Task<Procedure> GetByIdAsync(string id)
        {
            return await this.proceduresRepository
                .All()
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
