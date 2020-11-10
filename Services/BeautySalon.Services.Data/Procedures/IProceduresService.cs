namespace BeautySalon.Services.Data.Procedures
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IProceduresService
    {
        Task<string> CreateAsync(string name, string description, decimal price, string categoryId, string skinTypeId, string isSensitive, IList<SelectListItem> skinProblems);

        Task UpdateAsync(string id, string name, string description, decimal price, string categoryId, string skinTypeId, string isSensitive, IList<SelectListItem> skinProblems);

        Task DeleteAsync(string id);

        Task<Procedure> GetByIdAsync(string id);

        Task<IEnumerable<T>> GetAllByCategoryAsync<T>(string categoryId, int take, int skip);

        Task<T> GetProcedureDetailsAsync<T>(string id);

        Task<IEnumerable<T>> GetProcedureReviewsAsync<T>(string id);

        Task<IEnumerable<T>> GetProcedureProductsAsync<T>(string id);

        Task<IEnumerable<T>> SearchByAsync<T>(string skinTypeId, string criteria);

        Task<int> GetTotalCountProceduresByCategoryAsync(string categoryId);

        Task<IEnumerable<T>> GetProceduresByStylistAsync<T>(string stylistId);

        Task<IEnumerable<T>> GetSmartSearchProceduresAsync<T>(string clientSkinTypeId, string isSkinSensitive, string stylistId);

        Task<IEnumerable<T>> GetAllAdministrationAsync<T>();

        Task<T> GetProcedureDataForUpdateAsync<T>(string id);

        Task<string> GetProcedureIdByNameAsync(string procedureName);

        Task<T> GetProcedureProductsAdministrationAsync<T>(string id);

        Task<bool> AddProductToProcedureAsync(string id, string productId);

        Task RemoveProductAsync(string productId, string procedureId);
    }
}
