namespace BeautySalon.Services.Data.Reviews
{
    using System.Threading.Tasks;

    public interface IReviewsService
    {
        Task<T> GetReviewAsync<T>(string appointmentId);
    }
}
