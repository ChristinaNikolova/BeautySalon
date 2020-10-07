namespace BeautySalon.Services.Data.Stylists
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;

    public interface IStylistsService
    {
        Task<ApplicationUser> UpdateStylistProfileAsync(string id, string categoryName, string jobTypeName, string descripion);

        Task<Category> GetStylistCategoryByNameAsync(string categoryName);

        Task<Category> GetStylistCategoryByIdAsync(string categoryId);

        Task<JobType> GetStylistJobTypeByNameAsync(string jobTypeName);

        Task<JobType> GetStylistJobTypeByIdAsync(string jobTypeId);

        Task<ApplicationUser> GetStylistByIdAsync(string id);
    }
}
