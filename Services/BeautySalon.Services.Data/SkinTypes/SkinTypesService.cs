namespace BeautySalon.Services.Data.SkinTypes
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class SkinTypesService : ISkinTypesService
    {
        private readonly IRepository<SkinType> skinTypesRepository;

        public SkinTypesService(IRepository<SkinType> skinTypesRepository)
        {
            this.skinTypesRepository = skinTypesRepository;
        }

        public async Task<string> CreateAsync(string name, string description)
        {
            var skinType = new SkinType()
            {
                Name = name,
                Description = description,
            };

            await this.skinTypesRepository.AddAsync(skinType);
            await this.skinTypesRepository.SaveChangesAsync();

            return skinType.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var skinType = await this.GetByIdAsync(id);

            skinType.IsDeleted = true;

            this.skinTypesRepository.Update(skinType);
            await this.skinTypesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string name, string description, string id)
        {
            var skinType = await this.GetByIdAsync(id);

            skinType.Name = name;
            skinType.Description = description;

            this.skinTypesRepository.Update(skinType);
            await this.skinTypesRepository.SaveChangesAsync();
        }

        public async Task<SkinType> GetByIdAsync(string id)
        {
            return await this.skinTypesRepository
                .All()
                .FirstOrDefaultAsync(st => st.Id == id);
        }

        public async Task<SkinType> GetByNameAsync(string name)
        {
            return await this.skinTypesRepository
                .All()
                .FirstOrDefaultAsync(st => st.Name == name);
        }
    }
}
