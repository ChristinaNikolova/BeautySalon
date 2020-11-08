namespace BeautySalon.Services.Data.JobTypes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IJobTypesService
    {
        Task<string> CreateAsync(string name);

        Task EditAsync(string name, string id);

        Task DeleteAsync(string id);

        Task<JobType> GetByIdAsync(string id);

        Task<JobType> GetByNameAsync(string name);

        Task<IEnumerable<SelectListItem>> GetAllAsSelectListItemAsync();
    }
}
