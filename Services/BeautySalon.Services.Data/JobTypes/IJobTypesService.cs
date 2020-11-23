namespace BeautySalon.Services.Data.JobTypes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IJobTypesService
    {
        Task<IEnumerable<SelectListItem>> GetAllAsSelectListItemAsync();
    }
}
