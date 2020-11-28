namespace BeautySalon.Services.Data.Tests.Tests
{
    using System.Linq;
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

    public class ArticlesServiceTests : BaseServiceTests
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
            this.commentsRepository = new Mock<IRepository<Comment>>();
            this.clientArticleLikesRepository = new Mock<IRepository<ClientArticleLike>>();
            this.cloudinaryService = new Mock<ICloudinaryService>();

            this.stylist = new ApplicationUser() { Id = "10" };
            this.client = new ApplicationUser() { Id = "1" };
            this.category = new Category() { Id = "1" };
            this.comment = new Comment() { Id = "1" };
            this.mockPicture = new Mock<IFormFile>();
        }

        [Fact]
        public async Task CheckCreatingArticle()
        {
            var service = this.PrepareService();

            string articleId = await this.GetArticleAsync(service);

            Assert.NotNull(articleId);
        }

        [Fact]
        public async Task CheckGettingArticleDetails()
        {
            var service = this.PrepareService();

            string articleId = await this.GetArticleAsync(service);

            var articleDetails = await service.GetArticleDetailsAsync<TestArticleModel>(articleId);

            Assert.True(articleId.Equals(articleDetails.Id));
        }

        [Fact]
        public async Task CheckUpdatingArticle()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                this.clientArticleLikesRepository.Object,
                this.cloudinaryService.Object);

            string articleId = await this.GetArticleAsync(service);

            var pictureNew = this.mockPicture.Object;

            await service.UpdateAsync("new title", "content test article", "2", pictureNew, articleId);

            var updatedResult = await repository
                .All()
                .FirstOrDefaultAsync(a => a.Id == articleId);

            var getPictureUrl = await service.GetPictureUrlAsync(articleId);

            Assert.Same("new title", updatedResult.Title);
            Assert.Same("content test article", updatedResult.Content);
            Assert.Same("2", updatedResult.CategoryId);
            Assert.Same(getPictureUrl, updatedResult.Picture);
            Assert.Same(this.stylist.Id, updatedResult.StylistId);
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

            string articleId = await this.GetArticleAsync(service);

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

        [Fact]
        public async Task CheckingIfClientLikeTheArticle()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);
            var clientArticleLikesRepository = new EfRepository<ClientArticleLike>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                clientArticleLikesRepository,
                this.cloudinaryService.Object);

            string articleId = await this.GetArticleAsync(service);

            var likeCase = await service.LikeArticleAsync(articleId, this.client.Id);
            var isFavourite = await service.CheckFavouriteArticlesAsync(articleId, this.client.Id);
            var dislikeCase = await service.LikeArticleAsync(articleId, this.client.Id);

            Assert.True(likeCase);
            Assert.True(isFavourite);
            Assert.True(!dislikeCase);
        }

        [Fact]
        public async Task CheckGettingClientsFavouriteArtilcesAndCount()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);
            var clientArticleLikesRepository = new EfRepository<ClientArticleLike>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                clientArticleLikesRepository,
                this.cloudinaryService.Object);

            var firstArticleId = await this.GetArticleAsync(service);
            var secondArticleId = await this.GetArticleAsync(service);

            var articleLike = new ClientArticleLike()
            {
                ClientId = this.client.Id,
                ArticleId = firstArticleId,
            };
            var articleLikeSecond = new ClientArticleLike()
            {
                ClientId = this.client.Id,
                ArticleId = secondArticleId,
            };

            await clientArticleLikesRepository.AddAsync(articleLike);
            await clientArticleLikesRepository.AddAsync(articleLikeSecond);
            await clientArticleLikesRepository.SaveChangesAsync();

            var articles = await service.GetUsersFavouriteArticlesAsync<TestClientArticleLikesModel>(this.client.Id);
            var count = await service.GetLikesCountAsync(firstArticleId);

            Assert.Equal(2, articles.Count());
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckGettingAllArtcles()
        {
            var service = this.PrepareService();

            for (int i = 0; i < 3; i++)
            {
                await this.GetArticleAsync(service);
            }

            var articles = await service.GetAllAsync<TestArticleModel>(3, 0);
            var articlesCount = await service.GetTotalCountArticlesAsync();

            Assert.Equal(3, articles.Count());
            Assert.Equal(3, articlesCount);
        }

        [Fact]
        public async Task CheckGettingAllArtclesForCurrentStylist()
        {
            var service = this.PrepareService();

            for (int i = 0; i < 3; i++)
            {
                await this.GetArticleAsync(service);
            }

            var articlesResult = await service.GetAllForStylistAsync<TestArticleModel>(this.stylist.Id);

            Assert.Equal(3, articlesResult.Count());
        }

        [Fact]
        public async Task CheckGettingRecentArticles()
        {
            var service = this.PrepareService();

            for (int i = 0; i < 6; i++)
            {
                await this.GetArticleAsync(service);
            }

            var articles = await service.GetRecentArticlesAsync<TestArticleModel>();

            Assert.Equal(5, articles.Count());
        }

        [Fact]
        public async Task CheckSearchingArticleByGivenCategory()
        {
            var service = this.PrepareService();

            for (int i = 0; i < 3; i++)
            {
                await this.GetArticleAsync(service);
            }

            var picture = this.mockPicture.Object;

            for (int i = 0; i < 3; i++)
            {
                await service.CreateAsync("title", "content test article", "2", picture, this.stylist.Id);
            }

            var articles = await service.SearchByAsync<TestArticleModel>(this.category.Id);

            Assert.Equal(3, articles.Count());
        }

        private async Task<string> GetArticleAsync(ArticlesService service)
        {
            var picture = this.mockPicture.Object;

            var articleId = await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);

            return articleId;
        }

        private ArticlesService PrepareService()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);
            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                this.clientArticleLikesRepository.Object,
                this.cloudinaryService.Object);

            return service;
        }

        public class TestArticleModel : IMapFrom<Article>
        {
            public string Id { get; set; }
        }

        public class TestClientArticleLikesModel : IMapFrom<ClientArticleLike>
        {
            public string Id { get; set; }
        }
    }
}
