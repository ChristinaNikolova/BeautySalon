namespace BeautySalon.Services.Data.Users
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IUsersService
    {
        Task<ApplicationUser> UpdateUserProfileAsync(string id, string username, string firstName, string lastName, string address, string phoneNumber, string gender, string skinTypeName, string isSkinSensitive, IFormFile picture);

        Task<SkinType> GetUserSkinTypeByIdAsync(string skinTypeId);

        Task<SkinType> GetUserSkinTypeByNameAsync(string skinTypeName);

        Task<ApplicationUser> GetUserByIdAsync(string id);

        Task<T> GetUserDataAsync<T>(string userId);

        Task<T> GetUserSkinDataAsync<T>(string userId);

        Task AddSkinTypeDataAsync(string userId, bool isSkinSensitive, string skinTypeId, string[] skinProblems);
    }
}
