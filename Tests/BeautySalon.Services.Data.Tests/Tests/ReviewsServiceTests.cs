namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Reviews;
    using BeautySalon.Services.Mapping;
    using Xunit;

    public class ReviewsServiceTests : BaseServiceTests
    {
        public ReviewsServiceTests()
        {
        }

        [Fact]
        public async Task CheckGettingReviewAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Review>(db);
            var service = new ReviewsService(repository);

            var review = new Review()
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = "1",
                AppointmentId = "1",
                Content = "test content",
                Points = 5,
                ProcedureId = "1",
            };

            await repository.AddAsync(review);
            await repository.SaveChangesAsync();

            var reviewResult = await service.GetReviewAsync<TestReviewModel>("1");

            Assert.Equal(review.Content, reviewResult.Content);
        }

        public class TestReviewModel : IMapFrom<Review>
        {
            public string Content { get; set; }
        }
    }
}
