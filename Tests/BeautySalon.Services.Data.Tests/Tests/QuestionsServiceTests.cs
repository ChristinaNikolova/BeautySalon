namespace BeautySalon.Services.Data.Tests.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Questions;
    using BeautySalon.Services.Mapping;
    using Xunit;

    public class QuestionsServiceTests : BaseServiceTests
    {
        private readonly ApplicationUser stylist;
        private readonly ApplicationUser client;
        private readonly Question question;

        public QuestionsServiceTests()
        {
            this.stylist = new ApplicationUser() { Id = "10", };
            this.client = new ApplicationUser() { Id = "1", };
            this.question = new Question() { Id = "1", };
        }

        [Fact]
        public async Task CheckCreatingQuestionAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Question>(db);
            var service = new QuestionsService(repository);

            await this.PrepareQuestionAsync(service);

            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task CheckGettingQuestionDetailsAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Question>(db);
            var service = new QuestionsService(repository);

            await repository.AddAsync(this.question);
            await repository.SaveChangesAsync();

            var questionResult = await service.GetQuestionDetailsAsync<TestQuestionModel>(this.question.Id);

            Assert.Equal(this.question.Id, questionResult.Id);
        }

        [Fact]
        public async Task CheckGettingNewQuestionForStylistAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Question>(db);
            var service = new QuestionsService(repository);

            await this.PrepareQuestionAsync(service);
            await this.PrepareQuestionAsync(service);

            var questions = await service.GetAllNewQuestionsForStylistAsync<TestQuestionModel>(this.stylist.Id);
            var questionCount = await service.GetNewQuestionsCountAsync(this.stylist.Id);

            Assert.Equal(2, questions.Count());
            Assert.Equal(2, questionCount);
        }

        private async Task PrepareQuestionAsync(QuestionsService service)
        {
            await service.CreateAsync("test title", "test content", this.stylist.Id, this.client.Id);
        }

        public class TestQuestionModel : IMapFrom<Question>
        {
            public string Id { get; set; }
        }
    }
}
