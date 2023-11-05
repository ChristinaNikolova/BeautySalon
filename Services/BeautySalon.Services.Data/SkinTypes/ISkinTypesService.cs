namespace BeautySalon.Services.Data.SkinTypes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ISkinTypesService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<T> GetSkinTypeResultAsync<T>(string skinTypeName);

        Task<IEnumerable<SelectListItem>> GetAllAsSelectListItemAsync();
    }
}
