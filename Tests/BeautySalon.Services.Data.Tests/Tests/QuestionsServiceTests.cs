﻿namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
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
        public QuestionsServiceTests()
        {
            new MapperInitializationProfile();
        }

        [Fact]
        public async Task CheckGettingTheSkinQuiz()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Question>(db);
            var service = new QuestionsService(repository);

            //var firstQuestion = new QuizQuestion() { Id = Guid.NewGuid().ToString() };
            //var secondQuestion = new QuizQuestion() { Id = Guid.NewGuid().ToString() };
            //var thirdQuestion = new QuizQuestion() { Id = Guid.NewGuid().ToString() };

            //await db.QuizQuestions.AddAsync(firstQuestion);
            //await db.QuizQuestions.AddAsync(secondQuestion);
            //await db.QuizQuestions.AddAsync(thirdQuestion);
            //await db.SaveChangesAsync();

            //var quiz = await service.GetQuizAsync<TestQuizModel>();

            Assert.Equal();
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
