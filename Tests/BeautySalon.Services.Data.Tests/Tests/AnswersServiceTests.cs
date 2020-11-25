namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Answers;
    using BeautySalon.Services.Mapping;
    using BeautySalon.Web.ViewModels.Answers.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class AnswersServiceTests
    {
        private readonly Mock<IRepository<Question>> questionsRepository;
        private readonly Question question;
        private readonly ApplicationUser stylist;
        private readonly ApplicationUser client;

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
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Answer>(db);
            var questionRepository = new EfDeletableEntityRepository<Question>(db);
            await this.AddQuestionAsync(questionRepository);

            var service = new AnswersService(repository, questionRepository);
            var answerId = await service.CreateAsync("title", "content", this.stylist.Id, this.client.Id, this.question.Id);

            Assert.NotNull(answerId);
        }

        [Fact]
        public async Task CheckForNewAnswerShoudReturnTrue()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Answer>(db);
            var questionRepository = new EfDeletableEntityRepository<Question>(db);
            await this.AddQuestionAsync(questionRepository);

            var service = new AnswersService(repository, questionRepository);
            await service.CreateAsync("title", "content", this.stylist.Id, this.client.Id, this.question.Id);

            var isNewAnswer = await service.CheckNewAnswerAsync(this.client.Id);

            Assert.True(isNewAnswer);
        }

        [Fact]
        public async Task CheckGettingAnswersForCurrentUser()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Answer>(db);
            var questionRepository = new EfDeletableEntityRepository<Question>(db);
            await this.AddQuestionAsync(questionRepository);

            var service = new AnswersService(repository, questionRepository);
            await service.CreateAsync("title", "content", this.stylist.Id, this.client.Id, this.question.Id);

            var result = await service.GetAllAnswersForUserAsync<TestExerciseModel>(this.client.Id);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task CheckGettingNewAnswersForCurrentUser()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Answer>(db);
            var questionRepository = new EfDeletableEntityRepository<Question>(db);
            await this.AddQuestionAsync(questionRepository);

            var service = new AnswersService(repository, questionRepository);
            await service.CreateAsync("title", "content", this.stylist.Id, this.client.Id, this.question.Id);

            var result = await service.GetAllNewAnswersForUserAsync<TestExerciseModel>(this.client.Id);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task CheckGettingAnswersForStylist()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Answer>(db);
            var questionRepository = new EfDeletableEntityRepository<Question>(db);
            await this.AddQuestionAsync(questionRepository);

            var service = new AnswersService(repository, questionRepository);
            await service.CreateAsync("title", "content", this.stylist.Id, this.client.Id, this.question.Id);

            var result = await service.GetAllForStylistAsync<TestExerciseModel>(this.client.Id);

            Assert.NotEmpty(result);
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

            var result = await service.GetAnswerDetailsAsync<TestExerciseModel>(answerId);

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

        public class TestExerciseModel : IMapFrom<Answer>
        {
            public string Id { get; set; }
        }
    }
}
