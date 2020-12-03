namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Cards;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class CardsServiceTests : BaseServiceTests
    {
        public CardsServiceTests()
        {
        }

        [Fact]
        public async Task CheckCreatingCardForOneYearAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Card>(db);
            var typeCardRespository = new EfDeletableEntityRepository<TypeCard>(db);

            var service = new CardsService(
                typeCardRespository,
                repository);

            var user = new ApplicationUser()
            {
                Id = "1",
            };

            var typeCard = new TypeCard()
            {
                Id = "1",
                Price = 50,
                Name = "Year",
            };

            await db.TypeCards.AddAsync(typeCard);
            await db.SaveChangesAsync();

            await service.CreateCardAsync(user.Id, typeCard.Price);

            var card = await repository
                .All()
                .FirstOrDefaultAsync();

            var expectedEndDate = DateTime.UtcNow.AddDays(GlobalConstants.DaysOneYear + 1).Date;
            var actualEndDate = card.EndDate.Date;

            Assert.NotNull(card);
            Assert.Equal(expectedEndDate, actualEndDate);
        }

        [Fact]
        public async Task CheckCreatingCardForOneMonthAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Card>(db);
            var typeCardRespository = new EfDeletableEntityRepository<TypeCard>(db);

            var service = new CardsService(
                typeCardRespository,
                repository);

            var user = new ApplicationUser()
            {
                Id = "1",
            };

            var typeCard = new TypeCard()
            {
                Id = "1",
                Price = 50,
                Name = "Month",
            };

            await db.TypeCards.AddAsync(typeCard);
            await db.SaveChangesAsync();

            await service.CreateCardAsync(user.Id, typeCard.Price);

            var card = await repository
                .All()
                .FirstOrDefaultAsync();

            var expectedEndDate = DateTime.UtcNow.AddDays(GlobalConstants.DaysOneMonth + 1).Date;
            var actualEndDate = card.EndDate.Date;

            Assert.NotNull(card);
            Assert.Equal(expectedEndDate, actualEndDate);
        }

        [Fact]
        public async Task CheckCreatingCardForOneWeekAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Card>(db);
            var typeCardRespository = new EfDeletableEntityRepository<TypeCard>(db);

            var service = new CardsService(
                typeCardRespository,
                repository);

            var user = new ApplicationUser()
            {
                Id = "1",
            };

            var typeCard = new TypeCard()
            {
                Id = "1",
                Price = 50,
                Name = "Week",
            };

            await db.TypeCards.AddAsync(typeCard);
            await db.SaveChangesAsync();

            await service.CreateCardAsync(user.Id, typeCard.Price);

            var card = await repository
                .All()
                .FirstOrDefaultAsync();

            var expectedEndDate = DateTime.UtcNow.AddDays(GlobalConstants.DaysOneWeek + 1).Date;
            var actualEndDate = card.EndDate.Date;

            Assert.NotNull(card);
            Assert.Equal(expectedEndDate, actualEndDate);
        }

        [Fact]
        public async Task CheckGettingActiveAndExpiredCardsAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Card>(db);
            var typeCardRespository = new EfDeletableEntityRepository<TypeCard>(db);

            var service = new CardsService(
                typeCardRespository,
                repository);

            var firstUser = new ApplicationUser()
            {
                Id = "1",
            };

            var secondUser = new ApplicationUser()
            {
                Id = "1",
            };

            var typeCard = new TypeCard()
            {
                Id = "1",
                Price = 50,
                Name = "Week",
            };

            await db.TypeCards.AddAsync(typeCard);
            await db.SaveChangesAsync();

            await service.CreateCardAsync(firstUser.Id, typeCard.Price);
            await service.CreateCardAsync(secondUser.Id, typeCard.Price);

            var activeCards = await service.GetActiveCardsAsync<TestCardModel>();
            var expiredCards = await service.GetExpiredCardsAsync<TestCardModel>();

            Assert.Equal(2, activeCards.Count());
            Assert.Empty(expiredCards);
        }

        public class TestCardModel : IMapFrom<Card>
        {
            public string Id { get; set; }
        }
    }
}
