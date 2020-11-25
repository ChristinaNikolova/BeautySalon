namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Cloudinary;
    using BeautySalon.Services.Data.Articles;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ArticlesServiceTests
    {
        private readonly ApplicationUser stylist;
        private readonly ApplicationUser client;
        private readonly Category category;
        private readonly Comment comment;
        private readonly Mock<IFormFile> mockPicture;

        private readonly Mock<IRepository<Comment>> commentsRepository;
        private readonly Mock<IRepository<ClientArticleLike>> clientArticleLikesRepository;
        private readonly Mock<ICloudinaryService> cloudinaryService;

        public ArticlesServiceTests()
        {
            new MapperInitializationProfile();
            this.commentsRepository = new Mock<IRepository<Comment>>();
            this.clientArticleLikesRepository = new Mock<IRepository<ClientArticleLike>>();
            this.cloudinaryService = new Mock<ICloudinaryService>();
            this.stylist = new ApplicationUser()
            {
                Id = "1",
            };
            this.client = new ApplicationUser()
            {
                Id = "1",
            };
            this.category = new Category()
            {
                Id = "1",
            };
            this.comment = new Comment()
            {
                Id = "1",
            };
            this.mockPicture = new Mock<IFormFile>();
        }

        [Fact]
        public async Task CheckCreatingArticle()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);
            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                this.clientArticleLikesRepository.Object,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            var articleId = await service.CreateAsync("title", "content test article", this.category.Id,
             picture, this.stylist.Id);

            Assert.NotNull(articleId);
        }

        [Fact]
        public async Task CheckDeletingArticle()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);
            var clientArticleLikesRepository = new EfRepository<ClientArticleLike>(db);
            var commentsRepository = new EfDeletableEntityRepository<Comment>(db);

            var service = new ArticlesService(
                repository,
                commentsRepository,
                clientArticleLikesRepository,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            var articleId = await service.CreateAsync("title", "content test article", this.category.Id,
             picture, this.stylist.Id);

            var article = await repository.All().FirstOrDefaultAsync(a => a.Id == articleId);
            article.Comments.Add(this.comment);

            var articleLike = new ClientArticleLike()
            {
                ClientId = this.client.Id,
                ArticleId = articleId,
            };

            await clientArticleLikesRepository.AddAsync(articleLike);
            await clientArticleLikesRepository.SaveChangesAsync();
            repository.Update(article);
            await repository.SaveChangesAsync();

            await service.DeleteAsync(articleId);

            Assert.True(article.IsDeleted);
        }

        private static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            return db;
        }

        public class TestExerciseModel : IMapFrom<Article>
        {
            public string Id { get; set; }
        }
    }
}
