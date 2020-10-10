namespace BeautySalon.Services.Data.SkinTypes
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;

    public interface ISkinTypesService
    {
        Task<string> CreateAsync(string name, string description);

        Task EditAsync(string name, string description, string id);

        Task DeleteAsync(string id);

        Task<SkinType> GetByIdAsync(string id);

        Task<SkinType> GetByNameAsync(string name);
    }
}
