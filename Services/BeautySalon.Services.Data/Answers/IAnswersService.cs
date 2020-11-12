namespace BeautySalon.Services.Data.Answers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAnswersService
    {
        Task<string> CreateAsync(string title, string content, string questionStylistId, string questionClientId, string questionId);

        Task<IEnumerable<T>> GetAllForStylistAsync<T>(string stylistId);

        Task<T> GetAnswerDetailsAsync<T>(string id);
    }
}
