namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Questions;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class QuestionsServiceTests
    {
        private readonly ApplicationUser stylist;
        private readonly ApplicationUser client;

        public QuestionsServiceTests()
        {
            new MapperInitializationProfile();
            this.stylist = new ApplicationUser()
            {
                Id = "10",
            };
            this.client = new ApplicationUser()
            {
                Id = "1",
            };
        }

        [Fact]
        public async Task CheckCreatingQuestion()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Question>(db);
            var service = new QuestionsService(repository);

            await service.CreateAsync("test title", "test content", this.stylist.Id, this.client.Id);

            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task CheckGettingQuestionDetails()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Question>(db);
            var service = new QuestionsService(repository);

            var question = new Question()
            {
                Id = "1",
                Title = "title",
                Content = "Content",
                ClientId = this.client.Id,
                StylistId = this.stylist.Id,
            };

            await repository.AddAsync(question);
            await repository.SaveChangesAsync();

            var questionResult = await service.GetQuestionDetailsAsync<TestQuestionModel>(question.Id);

            Assert.Equal(question.Id, questionResult.Id);
        }

        [Fact]
        public async Task CheckGettingNewQuestionForStylist()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Question>(db);
            var service = new QuestionsService(repository);

            await service.CreateAsync("test title", "test content", this.stylist.Id, this.client.Id);
            await service.CreateAsync("test title 2", "test content 2", this.stylist.Id, this.client.Id);

            var questions = await service.GetAllNewQuestionsForStylistAsync<TestQuestionModel>(this.stylist.Id);
            var questionCount = await service.GetNewQuestionsCountAsync(this.stylist.Id);

            Assert.Equal(2, questions.Count());
            Assert.Equal(2, questionCount);
        }

        private static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            return db;
        }

        public class TestQuestionModel : IMapFrom<Question>
        {
            public string Id { get; set; }
        }
    }
}
