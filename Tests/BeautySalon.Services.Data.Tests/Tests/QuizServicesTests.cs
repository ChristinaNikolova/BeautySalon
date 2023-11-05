namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Quiz;
    using BeautySalon.Services.Mapping;
    using Xunit;

    public class QuizServicesTests : BaseServiceTests
    {
        public QuizServicesTests()
        {
        }

        [Fact]
        public async Task CheckGettingTheSkinQuizAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<QuizQuestion>(db);
            var service = new QuizService(repository);

            var firstQuestion = new QuizQuestion() { Id = Guid.NewGuid().ToString() };
            var secondQuestion = new QuizQuestion() { Id = Guid.NewGuid().ToString() };
            var thirdQuestion = new QuizQuestion() { Id = Guid.NewGuid().ToString() };

            await db.QuizQuestions.AddAsync(firstQuestion);
            await db.QuizQuestions.AddAsync(secondQuestion);
            await db.QuizQuestions.AddAsync(thirdQuestion);
            await db.SaveChangesAsync();

            var quiz = await service.GetQuizAsync<TestQuizModel>();

            Assert.Equal(3, quiz.Count());
        }

        public class TestQuizModel : IMapFrom<QuizQuestion>
        {
            public string Id { get; set; }
        }
    }
}
