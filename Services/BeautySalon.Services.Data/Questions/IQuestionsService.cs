namespace BeautySalon.Services.Data.Questions
{
    using System.Threading.Tasks;

    public interface IQuestionsService
    {
        Task CreateAsync(string title, string content, string stylistId, string userId);
    }
}
