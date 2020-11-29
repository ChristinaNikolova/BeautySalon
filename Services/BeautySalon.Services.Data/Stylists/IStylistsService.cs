namespace BeautySalon.Services.Data.Stylists
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IStylistsService
    {
        Task<string> AddRoleStylistAsync(string email);

        Task<ApplicationUser> UpdateStylistProfileAsync(string id, string firstName, string lastName, string phoneNumber, string category, string jobType, string descripion, IFormFile newPicture);

        Task<ApplicationUser> GetByIdAsync(string id);

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<T> GetStylistDetailsAsync<T>(string id);

        Task<IEnumerable<T>> SearchByCategoryAsync<T>(string categoryId);

        Task<string> GetPictureUrlAsync(string id);

        Task RemoveProcedureAsync(string stylistId, string procedureId);

        Task RemoveAllProceduresAsync(string id);

        Task<bool> AddProcedureToStylistAsync(string id, string procedureId);
    }
}
