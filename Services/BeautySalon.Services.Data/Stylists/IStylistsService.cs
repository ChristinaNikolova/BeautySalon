namespace BeautySalon.Services.Data.Stylists
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IStylistsService
    {
        Task<string> AddRoleStylistAsync(string email);

        Task<T> GetStylistDataForUpdateAsync<T>(string id);

        Task<ApplicationUser> UpdateStylistProfileAsync(string id, string firstName, string lastName, string phoneNumber, string category, string jobType, string descripion, IFormFile newPicture);

        Task<ApplicationUser> GetByIdAsync(string id);

        Task DeleteAsync(string id);

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<T> GetStylistDetailsAsync<T>(string id);

        Task<IEnumerable<T>> SearchByAsync<T>(string categoryId, string criteria);

        Task<T> GetStylistNamesAsync<T>(string id);

        Task<IEnumerable<T>> GetStylistsByCategoryAsync<T>(string categoryId);

        Task<IEnumerable<T>> GetAllAdministrationAsync<T>();
    }
}
