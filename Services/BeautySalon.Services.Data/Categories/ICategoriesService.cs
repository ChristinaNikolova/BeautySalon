namespace BeautySalon.Services.Data.Categories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICategoriesService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<SelectListItem>> GetAllAsSelectListItemAsync();

        Task<Category> GetByNameAsync(string name);

        Task<Category> GetByIdAsync(string id);
    }
}
