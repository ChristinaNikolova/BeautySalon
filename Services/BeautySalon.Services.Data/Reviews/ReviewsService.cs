namespace BeautySalon.Services.Data.Reviews
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ReviewsService : IReviewsService
    {
        private readonly IRepository<Review> reviewsRepository;

        public ReviewsService(IRepository<Review> reviewsRepository)
        {
            this.reviewsRepository = reviewsRepository;
        }

        public async Task<T> GetReviewAsync<T>(string appointmentId)
        {
            var review = await this.reviewsRepository
                .All()
                .Where(r => r.AppointmentId == appointmentId)
                .To<T>()
                .FirstOrDefaultAsync();

            return review;
        }
    }
}
