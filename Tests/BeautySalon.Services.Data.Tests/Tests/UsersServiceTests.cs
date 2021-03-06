﻿namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Linq;
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
    using Moq;
    using Xunit;

    public class UsersServiceTests : BaseServiceTests
    {
        private readonly Mock<IFormFile> mockPicture;
        private readonly ApplicationUser client;

        private readonly Mock<IRepository<SkinProblem>> skinProblemsRepository;
        private readonly Mock<IRepository<ClientSkinProblem>> clientSkinProblemsRepository;
        private readonly Mock<IRepository<Card>> cardsRepository;
        private readonly Mock<ICloudinaryService> cloudinaryService;

        public UsersServiceTests()
        {
            this.skinProblemsRepository = new Mock<IRepository<SkinProblem>>();
            this.clientSkinProblemsRepository = new Mock<IRepository<ClientSkinProblem>>();
            this.cardsRepository = new Mock<IRepository<Card>>();
            this.cloudinaryService = new Mock<ICloudinaryService>();
            this.mockPicture = new Mock<IFormFile>();
            this.client = new ApplicationUser() { Id = "1", };
        }

        [Fact]
        public async Task CheckUpdatingUserProfileAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var service = new UsersService(
                repository,
                this.skinProblemsRepository.Object,
                this.clientSkinProblemsRepository.Object,
                this.cardsRepository.Object,
                this.cloudinaryService.Object);

            var user = await AddUserToDbAsync(db);

            var picture = this.mockPicture.Object;

            var updatedUser = await service.UpdateUserProfileAsync(user.Id, "userName", user.FirstName, "new lastName", "new address", "1223", Gender.Female.ToString(), picture);

            Assert.Equal(user.FirstName, updatedUser.FirstName);
            Assert.Equal("new lastName", updatedUser.LastName);
        }

        [Fact]
        public async Task CheckGettingUserDataAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var service = new UsersService(
                repository,
                this.skinProblemsRepository.Object,
                this.clientSkinProblemsRepository.Object,
                this.cardsRepository.Object,
                this.cloudinaryService.Object);

            var user = await AddUserToDbAsync(db);

            var userData = await service.GetUserDataAsync<TestUserModel>(user.Id);

            Assert.Equal(user.Id, userData.Id);
        }

        [Fact]
        public async Task CheckAddingSkinTypeAndNonExistingSkinProblemsToUserAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var skinProblemsRepository = new EfDeletableEntityRepository<SkinProblem>(db);
            var clientSkinProblemsRepository = new EfRepository<ClientSkinProblem>(db);

            var service = new UsersService(
                repository,
                skinProblemsRepository,
                clientSkinProblemsRepository,
                this.cardsRepository.Object,
                this.cloudinaryService.Object);

            var skinType = new SkinType() { Id = "1" };

            var firstSkinProblem = new SkinProblem()
            {
                Id = "1",
                Name = "first problem",
            };

            var secondSkinProblem = new SkinProblem()
            {
                Id = "2",
                Name = "second problem",
            };

            var thirdSkinProblem = new SkinProblem()
            {
                Id = "3",
                Name = "third problem",
            };

            SkinProblem[] skinProblems = new SkinProblem[]
            {
                firstSkinProblem,
                secondSkinProblem,
                thirdSkinProblem,
            };

            await db.SkinProblems.AddRangeAsync(skinProblems);
            await db.Users.AddAsync(this.client);
            await db.SkinTypes.AddAsync(skinType);
            await db.SaveChangesAsync();

            string[] skinProblemsNames = new string[]
          {
                firstSkinProblem.Name,
                secondSkinProblem.Name,
                thirdSkinProblem.Name,
          };

            await service.AddSkinTypeDataAsync(this.client.Id, true, skinType.Id, skinProblemsNames);
            await service.AddSkinTypeDataAsync(this.client.Id, true, skinType.Id, skinProblemsNames);

            Assert.True(this.client.IsSkinSensitive);
            Assert.Equal(3, this.client.ClientSkinProblems.Count());
        }

        [Fact]
        public async Task CheckGettingUserCardAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var cardRepository = new EfDeletableEntityRepository<Card>(db);

            var service = new UsersService(
                repository,
                this.skinProblemsRepository.Object,
                this.clientSkinProblemsRepository.Object,
                cardRepository,
                this.cloudinaryService.Object);

            var user = await AddUserToDbAsync(db);

            var card = new Card()
            {
                Id = "1",
                ClientId = user.Id,
                IsPaid = true,
                EndDate = DateTime.UtcNow.AddDays(2),
            };

            await db.Cards.AddAsync(card);
            await db.SaveChangesAsync();

            var cardData = await service.GetUserCardAsync<TestUserCardModel>(user.Id);

            Assert.Equal(card.Id, cardData.Id);
        }

        [Fact]
        public async Task CheckGettingUsernameByIdAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);

            var service = new UsersService(
                repository,
                this.skinProblemsRepository.Object,
                this.clientSkinProblemsRepository.Object,
                this.cardsRepository.Object,
                this.cloudinaryService.Object);

            var user = await AddUserToDbAsync(db);

            var username = await service.GetUsernameByIdAsync(user.Id);

            Assert.Equal(user.UserName, username);
        }

        private static async Task<ApplicationUser> AddUserToDbAsync(ApplicationDbContext db)
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                FirstName = "firstName",
                LastName = "lastName",
                UserName = "username",
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return user;
        }

        public class TestUserModel : IMapFrom<ApplicationUser>
        {
            public string Id { get; set; }
        }

        public class TestUserCardModel : IMapFrom<Card>
        {
            public string Id { get; set; }
        }
    }
}
