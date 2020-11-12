namespace BeautySalon.Services.Data.Answers
{
    using System.Threading.Tasks;

    public interface IAnswersService
    {
        Task<string> CreateAsync(string title, string content, string questionStylistId, string questionClientId, string questionId);
    }
}
