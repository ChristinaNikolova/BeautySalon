namespace BeautySalon.Services.Data.Answers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAnswersService
    {
        Task<string> CreateAsync(string title, string content, string questionStylistId, string questionClientId, string questionId);

        Task<IEnumerable<T>> GetAllForStylistAsync<T>(string stylistId);

        Task<IEnumerable<T>> GetAllAnswersForUserAsync<T>(string userId);

        Task<IEnumerable<T>> GetAllNewAnswersForUserAsync<T>(string userId);

        Task<T> GetAnswerDetailsAsync<T>(string id);

        Task<bool> CheckNewAnswerAsync(string userId);

        Task ChangeIsRedAsync(string id);
    }
}
