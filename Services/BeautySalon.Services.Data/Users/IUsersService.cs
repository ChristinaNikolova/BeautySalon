namespace BeautySalon.Services.Data.Users
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IUsersService
    {
        Task<ApplicationUser> UpdateUserProfileAsync(string id, string username, string firstName, string lastName, string address, string phoneNumber, string gender, IFormFile newPicture);

        Task<ApplicationUser> GetUserByIdAsync(string id);

        Task<T> GetUserDataAsync<T>(string userId);

        Task AddSkinTypeDataAsync(string userId, bool isSkinSensitive, string skinTypeId, string[] skinProblems);

        Task<bool> HasSubscriptionCardAsync(string userId);

        Task<T> GetUserCardAsync<T>(string userId);
    }
}
