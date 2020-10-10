namespace BeautySalon.Services.Data.SkinProblems
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;

    public interface ISkinProblemsService
    {
        Task<string> CreateAsync(string name, string description);

        Task EditAsync(string name, string description, string id);

        Task DeleteAsync(string id);

        Task<SkinProblem> GetByIdAsync(string id);
    }
}
