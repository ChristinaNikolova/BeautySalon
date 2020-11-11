namespace BeautySalon.Services.Data.Questions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IQuestionsService
    {
        Task CreateAsync(string title, string content, string stylistId, string userId);

        Task<IEnumerable<T>> GetAllForStylistAsync<T>(string stylistId);
    }
}
