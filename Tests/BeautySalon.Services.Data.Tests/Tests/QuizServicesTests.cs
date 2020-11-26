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
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class QuizServicesTests
    {
        public QuizServicesTests()
        {
            new MapperInitializationProfile();
        }

        [Fact]
        public async Task CheckGettingTheSkinQuiz()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Question>(db);
            var service = new QuizService(repository);

            var firstQuestion = new Question() { Id = Guid.NewGuid().ToString() };
            var secondQuestion = new Question() { Id = Guid.NewGuid().ToString() };
            var thirdQuestion = new Question() { Id = Guid.NewGuid().ToString() };

            await db.QuizQuestions.AddAsync(firstQuestion);
            await db.QuizQuestions.AddAsync(secondQuestion);
            await db.QuizQuestions.AddAsync(thirdQuestion);
            await db.SaveChangesAsync();

            var quiz = await service.GetQuizAsync<TestQuizModel>();

            Assert.Equal(3, quiz.Count());
        }

        private static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            return db;
        }

        public class TestQuizModel : IMapFrom<Question>
        {
            public string Id { get; set; }
        }
    }
}
