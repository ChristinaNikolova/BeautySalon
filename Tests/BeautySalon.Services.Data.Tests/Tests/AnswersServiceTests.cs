namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Answers;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class AnswersServiceTests
    {
        private readonly Question question;
        private readonly ApplicationUser stylist;
        private readonly ApplicationUser client;

        private readonly Mock<IRepository<Question>> questionsRepository;

        public AnswersServiceTests()
        {
            new MapperInitializationProfile();
            this.questionsRepository = new Mock<IRepository<Question>>();
            this.stylist = new ApplicationUser()
            {
                Id = "1",
            };
            this.client = new ApplicationUser()
            {
                Id = "1",
            };
            this.question = new Question()
            {
                Id = "1",
            };
        }

        [Fact]
        public async Task AnswerIsRedPropertyShoudChangeToTrue()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Answer>(db);
            var service = new AnswersService(repository, this.questionsRepository.Object);
            var answer = new Answer() { Id = Guid.NewGuid().ToString() };

            await db.Answers.AddAsync(answer);
            await db.SaveChangesAsync();

            await service.ChangeIsRedAsync(answer.Id);

            Assert.True(answer.IsRed);
        }

        [Fact]
        public async Task CheckCreatingNewAnswer()
        {
            AnswersService service = await this.ArrangeAnswersService();

            var answerId = await service.CreateAsync("title", "content", this.stylist.Id, this.client.Id, this.question.Id);

            Assert.NotNull(answerId);
        }

        [Fact]
        public async Task CheckForNewAnswerShoudReturnTrue()
        {
            AnswersService service = await this.ArrangeAnswersService();

            var isNewAnswer = await service.CheckNewAnswerAsync(this.client.Id);

            Assert.True(isNewAnswer);
        }

        [Fact]
        public async Task CheckGettingAnswersForUser()
        {
            AnswersService service = await this.ArrangeAnswersService();

            var resultUserSideOnlyNewAnswers = await service.GetAllNewAnswersForUserAsync<TestAnswerModel>(this.client.Id);
            var resultUserSide = await service.GetAllAnswersForUserAsync<TestAnswerModel>(this.client.Id);
            var resultStylistSide = await service.GetAllForStylistAsync<TestAnswerModel>(this.stylist.Id);

            Assert.NotEmpty(resultUserSideOnlyNewAnswers);
            Assert.NotEmpty(resultUserSide);
            Assert.NotEmpty(resultStylistSide);
        }

        [Fact]
        public async Task CheckGettingAnswersDetails()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Answer>(db);
            var questionRepository = new EfDeletableEntityRepository<Question>(db);
            await this.AddQuestionAsync(questionRepository);

            var service = new AnswersService(repository, questionRepository);
            var answerId = await service.CreateAsync("title", "content", this.stylist.Id, this.client.Id, this.question.Id);

            var result = await service.GetAnswerDetailsAsync<TestAnswerModel>(answerId);

            Assert.NotNull(result);
        }

        private static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            return db;
        }

        private async Task AddQuestionAsync(EfDeletableEntityRepository<Question> questionRepository)
        {
            await questionRepository.AddAsync(this.question);
            await questionRepository.SaveChangesAsync();
        }

        private async Task<AnswersService> ArrangeAnswersService()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Answer>(db);
            var questionRepository = new EfDeletableEntityRepository<Question>(db);
            await this.AddQuestionAsync(questionRepository);

            var service = new AnswersService(repository, questionRepository);
            await service.CreateAsync("title", "content", this.stylist.Id, this.client.Id, this.question.Id);

            return service;
        }

        public class TestAnswerModel : IMapFrom<Answer>
        {
            public string Id { get; set; }
        }
    }
}
