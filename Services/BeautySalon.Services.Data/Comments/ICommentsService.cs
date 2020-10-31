namespace BeautySalon.Services.Data.Comments
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICommentsService
    {
        Task CreateAsync(string content, string articleId, string userId);

        Task<IEnumerable<T>> GetAllAsync<T>(string articleId);
    }
}
