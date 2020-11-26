namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
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

            var articleId = await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);

            Assert.NotNull(articleId);
        }

        [Fact]
        public async Task CheckGettingArticleDetailsAndDataForUpdate()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);
            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                this.clientArticleLikesRepository.Object,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            var articleId = await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);

            var articleDetails = await service.GetArticleDetailsAsync<TestArticleModel>(articleId);
            var articleUpdataDate = await service.GetDataForUpdateAsync<TestArticleModel>(articleId);

            var expected = await repository
                .All()
                .Where(a => a.Id == articleId)
                .To<TestArticleModel>()
                .FirstOrDefaultAsync();

            Assert.True(expected.Id.Equals(articleDetails.Id));
            Assert.True(expected.Id.Equals(articleUpdataDate.Id));
        }

        [Fact]
        public async Task CheckUpdatingArticleWithoutNewPicture()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                this.clientArticleLikesRepository.Object,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            var articleId = await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);

            await service.UpdateAsync("new title", "content test article", "2", null, articleId);

            var updatedResult = await repository
                .All()
                .FirstOrDefaultAsync(a => a.Id == articleId);

            var getPictureUrl = await service.GetPictureUrlAsync(articleId);

            var expectedResult = new Article()
            {
                Id = articleId,
                Title = "new title",
                Content = "content test article",
                CategoryId = "2",
                Picture = getPictureUrl,
                StylistId = this.stylist.Id,
            };

            Assert.Same(expectedResult.Id, updatedResult.Id);
            Assert.Same(expectedResult.Title, updatedResult.Title);
            Assert.Same(expectedResult.Content, updatedResult.Content);
            Assert.Same(expectedResult.CategoryId, updatedResult.CategoryId);
            Assert.Same(expectedResult.Picture, updatedResult.Picture);
            Assert.Same(expectedResult.StylistId, updatedResult.StylistId);
        }

        [Fact]
        public async Task CheckUpdatingArticleWithNewPicture()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                this.clientArticleLikesRepository.Object,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            var articleId = await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);

            var pictureNew = this.mockPicture.Object;

            await service.UpdateAsync("new title", "content test article", "2", pictureNew, articleId);

            var updatedResult = await repository
                .All()
                .FirstOrDefaultAsync(a => a.Id == articleId);

            var getPictureUrl = await service.GetPictureUrlAsync(articleId);

            var expectedResult = new Article()
            {
                Id = articleId,
                Title = "new title",
                Content = "content test article",
                CategoryId = "2",
                Picture = getPictureUrl,
                StylistId = this.stylist.Id,
            };

            Assert.Same(expectedResult.Id, updatedResult.Id);
            Assert.Same(expectedResult.Title, updatedResult.Title);
            Assert.Same(expectedResult.Content, updatedResult.Content);
            Assert.Same(expectedResult.CategoryId, updatedResult.CategoryId);
            Assert.Same(expectedResult.Picture, updatedResult.Picture);
            Assert.Same(expectedResult.StylistId, updatedResult.StylistId);
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

            var articleId = await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);

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
        public async Task CheckIfClientLikeCurrentArticle()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);
            var clientArticleLikesRepository = new EfRepository<ClientArticleLike>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                clientArticleLikesRepository,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            var articleId = await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);

            var articleLike = new ClientArticleLike()
            {
                ClientId = this.client.Id,
                ArticleId = articleId,
            };

            await clientArticleLikesRepository.AddAsync(articleLike);
            await clientArticleLikesRepository.SaveChangesAsync();

            var result = await service.CheckFavouriteArticlesAsync(articleId, this.client.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task CheckCalculatingAricleLikes()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);
            var clientArticleLikesRepository = new EfRepository<ClientArticleLike>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                clientArticleLikesRepository,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            var articleId = await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);

            var articleLike = new ClientArticleLike()
            {
                ClientId = this.client.Id,
                ArticleId = articleId,
            };
            var articleLikeSecond = new ClientArticleLike()
            {
                ClientId = "2",
                ArticleId = articleId,
            };

            await clientArticleLikesRepository.AddAsync(articleLike);
            await clientArticleLikesRepository.AddAsync(articleLikeSecond);
            await clientArticleLikesRepository.SaveChangesAsync();

            var count = await service.GetLikesCountAsync(articleId);

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task CheckingIfClientLikeTheArticleRemovingFromLikesCase()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);
            var clientArticleLikesRepository = new EfRepository<ClientArticleLike>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                clientArticleLikesRepository,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            var articleId = await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);

            var articleLike = new ClientArticleLike()
            {
                ClientId = this.client.Id,
                ArticleId = articleId,
            };

            await clientArticleLikesRepository.AddAsync(articleLike);
            await clientArticleLikesRepository.SaveChangesAsync();

            var result = await service.LikeArticleAsync(articleId, this.client.Id);

            Assert.True(!result);
        }

        [Fact]
        public async Task CheckingIfClientLikeTheArticleAddingToLikesCase()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);
            var clientArticleLikesRepository = new EfRepository<ClientArticleLike>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                clientArticleLikesRepository,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            var articleId = await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);

            var result = await service.LikeArticleAsync(articleId, this.client.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task CheckGettingClientsFavouriteArtilces()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);
            var clientArticleLikesRepository = new EfRepository<ClientArticleLike>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                clientArticleLikesRepository,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            var firstArticleId = await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);

            var secondArticleId = await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);

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

            Assert.Equal(2, articles.Count());
        }

        [Fact]
        public async Task CheckGettingAllArtcles()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                this.clientArticleLikesRepository.Object,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);
            await service.CreateAsync("title1", "content test article", this.category.Id, picture, this.stylist.Id);
            await service.CreateAsync("title2", "content test article", this.category.Id, picture, this.stylist.Id);

            var articles = await service.GetAllAsync<TestArticleModel>(3, 0);
            var articlesCount = await service.GetTotalCountArticlesAsync();

            Assert.Equal(3, articles.Count());
            Assert.Equal(3, articlesCount);
        }

        [Fact]
        public async Task CheckGettingAllArtclesForCurrentStylist()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                this.clientArticleLikesRepository.Object,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);
            await service.CreateAsync("title1", "content test article", this.category.Id, picture, this.stylist.Id);
            await service.CreateAsync("title2", "content test article", this.category.Id, picture, this.stylist.Id);

            var articlesResult = await service.GetAllForStylistAsync<TestArticleModel>(this.stylist.Id);

            Assert.Equal(3, articlesResult.Count());
        }

        [Fact]
        public async Task CheckGettingArticlePictureUrl()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                this.clientArticleLikesRepository.Object,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            var articleId = await service.CreateAsync("title", "content test article", this.category.Id, picture, this.stylist.Id);

            var pictureUrl = await service.GetPictureUrlAsync(articleId);

            var expectedPictureUrl = await repository
                .All()
                .Where(a => a.Id == articleId)
                .Select(a => a.Picture)
                .FirstOrDefaultAsync();

            Assert.Same(expectedPictureUrl, pictureUrl);
        }

        [Fact]
        public async Task CheckGettingRecentArticles()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                this.clientArticleLikesRepository.Object,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            await service.CreateAsync("title1", "content test article", this.category.Id, picture, this.stylist.Id);
            await service.CreateAsync("title2", "content test article", this.category.Id, picture, this.stylist.Id);
            await service.CreateAsync("title3", "content test article", this.category.Id, picture, this.stylist.Id);
            await service.CreateAsync("title4", "content test article", this.category.Id, picture, this.stylist.Id);
            await service.CreateAsync("title5", "content test article", this.category.Id, picture, this.stylist.Id);
            await service.CreateAsync("title6", "content test article", this.category.Id, picture, this.stylist.Id);

            var articles = await service.GetRecentArticlesAsync<TestArticleModel>();

            Assert.Equal(5, articles.Count());
        }

        [Fact]
        public async Task CheckSearchingArticleByGivenCategory()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Article>(db);

            var service = new ArticlesService(
                repository,
                this.commentsRepository.Object,
                this.clientArticleLikesRepository.Object,
                this.cloudinaryService.Object);

            var picture = this.mockPicture.Object;

            await service.CreateAsync("title1", "content test article", this.category.Id, picture, this.stylist.Id);
            await service.CreateAsync("title2", "content test article", this.category.Id, picture, this.stylist.Id);
            await service.CreateAsync("title3", "content test article", this.category.Id, picture, this.stylist.Id);
            await service.CreateAsync("title4", "content test article", "2", picture, this.stylist.Id);
            await service.CreateAsync("title5", "content test article", "2", picture, this.stylist.Id);
            await service.CreateAsync("title6", "content test article", "2", picture, this.stylist.Id);

            var articles = await service.SearchByAsync<TestArticleModel>(this.category.Id);

            Assert.Equal(3, articles.Count());
        }

        private static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            return db;
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
