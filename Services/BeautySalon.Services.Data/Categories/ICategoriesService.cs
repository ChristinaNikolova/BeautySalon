namespace BeautySalon.Services.Data.Categories
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;

    public interface ICategoriesService
    {
        Task<string> CreateAsync(string name);

        Task EditAsync(string name, string id);

        Task DeleteAsync(string id);

        Task<Category> GetByIdAsync(string id);

        Task<Category> GetByNameAsync(string name);
    }
}
