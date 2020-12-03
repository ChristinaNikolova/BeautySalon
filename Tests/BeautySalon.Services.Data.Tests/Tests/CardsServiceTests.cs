namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Cards;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class CardsServiceTests : BaseServiceTests
    {
        public CardsServiceTests()
        {
        }

        [Fact]
        public async Task CheckCreatingCardAsync()
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
                Id = "2",
            };

            var thirdUser = new ApplicationUser()
            {
                Id = "3",
            };

            var firstTypeCard = new TypeCard()
            {
                Id = "1",
                Price = 50,
                Name = "Month",
            };

            var secondTypeCard = new TypeCard()
            {
                Id = "2",
                Price = 20,
                Name = "Week",
            };

            var thirdTypeCard = new TypeCard()
            {
                Id = "3",
                Price = 100,
                Name = "Year",
            };

            await db.TypeCards.AddAsync(firstTypeCard);
            await db.TypeCards.AddAsync(secondTypeCard);
            await db.TypeCards.AddAsync(thirdTypeCard);
            await db.SaveChangesAsync();

            await service.CreateCardAsync(firstUser.Id, firstTypeCard.Price);
            await service.CreateCardAsync(secondUser.Id, secondTypeCard.Price);
            await service.CreateCardAsync(thirdUser.Id, thirdTypeCard.Price);

            var firstCard = await repository
                .All()
                .FirstOrDefaultAsync(c => c.ClientId == firstUser.Id);

            var secondCard = await repository
               .All()
               .FirstOrDefaultAsync(c => c.ClientId == secondUser.Id);

            var thirdCard = await repository
               .All()
               .FirstOrDefaultAsync(c => c.ClientId == thirdUser.Id);

            var expectedEndDateFirstCard = DateTime.UtcNow.AddDays(GlobalConstants.DaysOneMonth + 1).Date;
            var actualEndDateFirstCard = firstCard.EndDate.Date;

            var expectedEndDateSecondCard = DateTime.UtcNow.AddDays(GlobalConstants.DaysOneWeek + 1).Date;
            var actualEndDateSecondCard = secondCard.EndDate.Date;

            var expectedEndDateThirdCard = DateTime.UtcNow.AddDays(GlobalConstants.DaysOneYear + 1).Date;
            var actualEndDateThirdCard = thirdCard.EndDate.Date;

            Assert.NotNull(firstCard);
            Assert.Equal(expectedEndDateFirstCard, actualEndDateFirstCard);
            Assert.Equal(expectedEndDateSecondCard, actualEndDateSecondCard);
            Assert.Equal(expectedEndDateThirdCard, actualEndDateThirdCard);
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
