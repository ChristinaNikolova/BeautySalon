namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Comments;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class CommentsServiceTests : BaseServiceTests
    {
        private readonly Article article;
        private readonly ApplicationUser client;

        public CommentsServiceTests()
        {
            this.article = new Article()
            {
                Id = "1",
                Title = "test Title",
            };

            this.client = new ApplicationUser()
            {
                Id = "1",
                UserName = "testUserName",
            };
        }

        [Fact]
        public async Task CheckCreatingCommentAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Comment>(db);
            var service = new CommentsService(repository);

            await this.PrepareCommentAsync(service);

            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task CheckDeletingCommentAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Comment>(db);
            var service = new CommentsService(repository);

            await this.PrepareCommentAsync(service);

            var comment = await repository
                .All()
                .FirstOrDefaultAsync(c => c.Content == "test content");

            await service.DeleteAsync(comment.Id);

            Assert.True(comment.IsDeleted);
        }

        [Fact]
        public async Task CheckGettingAllCommentsAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Comment>(db);
            var service = new CommentsService(repository);

            for (int i = 0; i < 3; i++)
            {
                await this.PrepareCommentAsync(service);
            }

            await service.CreateAsync("test content", "2", this.client.Id);

            var comments = await service.GetAllAsync<TestCommentModel>(this.article.Id);

            Assert.Equal(3, comments.Count());
        }

        [Fact]
        public async Task CheckGettingAllCommentsFromPreviuosDayAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Comment>(db);
            var service = new CommentsService(repository);

            var firstComment = new Comment()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.UtcNow.Date.AddDays(-1),
                Content = "test content 1",
                ClientId = this.client.Id,
                ArticleId = this.article.Id,
            };

            var secondComment = new Comment()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.UtcNow.Date.AddDays(-1),
                Content = "test content 2",
                ClientId = this.client.Id,
                ArticleId = this.article.Id,
            };

            var thirdComment = new Comment()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.UtcNow,
                Content = "test content 3",
                ClientId = this.client.Id,
                ArticleId = this.article.Id,
            };

            await db.Users.AddAsync(this.client);
            await db.Articles.AddAsync(this.article);
            await db.Comments.AddAsync(firstComment);
            await db.Comments.AddAsync(secondComment);
            await db.Comments.AddAsync(thirdComment);
            await db.SaveChangesAsync();

            var comments = await service.GetAllFromPreviousDayAsync<TestCommentModel>();

            Assert.Equal(2, comments.Count());
        }

        private async Task PrepareCommentAsync(CommentsService service)
        {
            await service.CreateAsync("test content", this.article.Id, this.client.Id);
        }

        public class TestCommentModel : IMapFrom<Comment>
        {
            public string Id { get; set; }
        }
    }
}
