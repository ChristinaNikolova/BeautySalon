namespace BeautySalon.Services.Data.Categories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICategoriesService
    {
        Task<string> CreateAsync(string name);

        Task EditAsync(string name, string id);

        Task DeleteAsync(string id);

        Task<Category> GetByIdAsync(string id);

        Task<Category> GetByNameAsync(string name);

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<SelectListItem>> GetAllAsSelectListItemAsync();
    }
}
