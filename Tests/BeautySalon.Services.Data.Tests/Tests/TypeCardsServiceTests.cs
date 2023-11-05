namespace BeautySalon.Services.Data.Tests.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.TypeCards;
    using BeautySalon.Services.Mapping;
    using Xunit;

    public class TypeCardsServiceTests : BaseServiceTests
    {
        public TypeCardsServiceTests()
        {
        }

        [Fact]
        public async Task CheckGettingAllAsync()
        {
            TypeCardsService service = await PrepareService();

            var typeCards = await service.GetAllAsync<TestTypeCardModel>();

            Assert.Equal(2, typeCards.Count());
        }

        [Fact]
        public async Task CheckGettingPriceAsync()
        {
            TypeCardsService service = await PrepareService();

            var price = await service.GetPriceAsync("1");

            Assert.Equal(50, price);
        }

        private static async Task<TypeCardsService> PrepareService()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<TypeCard>(db);
            var service = new TypeCardsService(repository);

            var firstTypeCard = new TypeCard()
            {
                Id = "1",
                Price = 50,
            };

            var secondTypeCard = new TypeCard()
            {
                Id = "2",
                Price = 250,
            };

            await db.TypeCards.AddAsync(firstTypeCard);
            await db.TypeCards.AddAsync(secondTypeCard);
            await db.SaveChangesAsync();
            return service;
        }

        public class TestTypeCardModel : IMapFrom<TypeCard>
        {
            public string Id { get; set; }
        }
    }
}
