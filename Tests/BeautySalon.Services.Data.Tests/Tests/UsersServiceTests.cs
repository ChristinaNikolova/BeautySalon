namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Models.Enums;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Cloudinary;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class UsersServiceTests
    {
        private readonly Mock<IFormFile> mockPicture;

        private readonly Mock<IRepository<SkinProblem>> skinProblemsRepository;
        private readonly Mock<IRepository<ClientSkinProblem>> clientSkinProblemsRepository;
        private readonly Mock<ICloudinaryService> cloudinaryService;

        public UsersServiceTests()
        {
            new MapperInitializationProfile();
            this.skinProblemsRepository = new Mock<IRepository<SkinProblem>>();
            this.clientSkinProblemsRepository = new Mock<IRepository<ClientSkinProblem>>();
            this.cloudinaryService = new Mock<ICloudinaryService>();
            this.mockPicture = new Mock<IFormFile>();
        }

        [Fact]
        public async Task CheckUpdatingUserProfile()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var service = new UsersService(
                repository,
                this.skinProblemsRepository.Object,
                this.clientSkinProblemsRepository.Object,
                this.cloudinaryService.Object);

            var user = new ApplicationUser()
            {
                Id = "1",
                FirstName = "firstName",
                LastName = "lastName",
                UserName = "username",
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var picture = this.mockPicture.Object;

            var updatedUser = await service.UpdateUserProfileAsync(user.Id, "userName", user.FirstName, "new lastName", "new address", "1223", Gender.Female.ToString(), picture);

            Assert.Equal(user.FirstName, updatedUser.FirstName);
            Assert.Equal("new lastName", updatedUser.LastName);
        }

        private static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            return db;
        }

        public class TestUserModel : IMapFrom<ApplicationUser>
        {
            public string Id { get; set; }
        }
    }
}
