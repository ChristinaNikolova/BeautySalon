namespace BeautySalon.Services.Data.Brands
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IBrandsService
    {
        Task<IEnumerable<SelectListItem>> GetAllAsSelectListItemAsync();
    }
}
