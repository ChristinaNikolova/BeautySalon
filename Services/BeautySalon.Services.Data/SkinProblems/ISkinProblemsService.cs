namespace BeautySalon.Services.Data.SkinProblems
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ISkinProblemsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IList<SelectListItem>> GetAllAsSelectListItemAsync();
    }
}
