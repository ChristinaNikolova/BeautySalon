namespace BeautySalon.Services.Data.SkinTypes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ISkinTypesService
    {
        Task<string> CreateAsync(string name, string description);

        Task EditAsync(string name, string description, string id);

        Task DeleteAsync(string id);

        Task<SkinType> GetByIdAsync(string id);

        Task<SkinType> GetByNameAsync(string name);

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<T> GetSkinTypeResultAsync<T>(string skinTypeName);

        Task<IEnumerable<SelectListItem>> GetAllAsSelectListItemAsync();
    }
}
