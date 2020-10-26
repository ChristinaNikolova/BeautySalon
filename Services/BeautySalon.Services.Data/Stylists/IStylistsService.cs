namespace BeautySalon.Services.Data.Stylists
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;

    public interface IStylistsService
    {
        Task<string> AddRoleStylistAsync(string username, string email);

        Task<ApplicationUser> UpdateStylistProfileAsync(string id, string categoryName, string jobTypeName, string descripion);

        Task<ApplicationUser> GetByIdAsync(string id);

        Task DeleteAsync(string id);

        Task<IEnumerable<T>> GetAllAsync<T>();
    }
}
