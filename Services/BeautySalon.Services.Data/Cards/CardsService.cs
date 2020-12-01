namespace BeautySalon.Services.Data.Cards
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class CardsService : ICardsService
    {
        private readonly IRepository<TypeCard> typeCardsRepository;
        private readonly IRepository<Card> cardsRepository;

        public CardsService(
            IRepository<TypeCard> typeCardsRepository,
            IRepository<Card> cardsRepository)
        {
            this.typeCardsRepository = typeCardsRepository;
            this.cardsRepository = cardsRepository;
        }

        public async Task CreateCardAsync(string userId, int price)
        {
            var typeCard = await this.typeCardsRepository
                .All()
                .Where(tc => tc.Price == price)
                .FirstOrDefaultAsync();

            var card = new Card()
            {
                ClientId = userId,
                IsPaid = true,
                TypeCardId = typeCard.Id,
            };

            if (typeCard.Name.ToLower() == "year")
            {
                card.EndEnd = card.StartDate.AddDays(365);
            }
            else if (typeCard.Name.ToLower() == "month")
            {
                card.EndEnd = card.StartDate.AddDays(30);
            }
            else
            {
                card.EndEnd = card.StartDate.AddDays(7);
            }

            await this.cardsRepository.AddAsync(card);
            await this.cardsRepository.SaveChangesAsync();
        }
    }
}
