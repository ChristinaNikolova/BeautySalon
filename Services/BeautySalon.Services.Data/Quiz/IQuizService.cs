namespace BeautySalon.Services.Data.Quiz
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IQuizService
    {
        Task<IEnumerable<T>> GetQuizAsync<T>();
    }
}
