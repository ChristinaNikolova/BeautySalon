namespace BeautySalon.Services.Data.SkinTypes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class SkinTypesService : ISkinTypesService
    {
        private readonly string sensitiveSkin = "sensitive";

        private readonly IRepository<SkinType> skinTypesRepository;

        public SkinTypesService(IRepository<SkinType> skinTypesRepository)
        {
            this.skinTypesRepository = skinTypesRepository;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var skinTypes = await this.skinTypesRepository
                .All()
                .Where(st => st.Name.ToLower() != this.sensitiveSkin)
                .OrderBy(st => st.Name)
                .To<T>()
                .ToListAsync();

            return skinTypes;
        }

        public async Task<T> GetSkinTypeResultAsync<T>(string skinTypeName)
        {
            var result = await this.skinTypesRepository
                .All()
                .Where(st => st.Name == skinTypeName)
                .To<T>()
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllAsSelectListItemAsync()
        {
            var skinTypes = await this.skinTypesRepository
                .All()
                .Where(st => st.Name.ToLower() != this.sensitiveSkin)
                .Select(c => new SelectListItem()
                {
                    Value = c.Id,
                    Text = c.Name,
                })
                .ToListAsync();

            return skinTypes;
        }
    }
}
