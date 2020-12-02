namespace BeautySalon.Services.Data.Cards
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
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

            GetCardPeriod(typeCard, card);

            await this.cardsRepository.AddAsync(card);
            await this.cardsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetActiveCardsAsync<T>()
        {
            var activeCards = await this.cardsRepository
                .All()
                .Where(c => c.EndEnd >= DateTime.UtcNow)
                .OrderBy(c => c.EndEnd)
                .To<T>()
                .ToListAsync();

            return activeCards;
        }

        public async Task<IEnumerable<T>> GetExpiredCardsAsync<T>()
        {
            var expiredCards = await this.cardsRepository
               .All()
               .Where(c => c.EndEnd < DateTime.UtcNow)
               .OrderByDescending(c => c.EndEnd)
               .To<T>()
               .ToListAsync();

            return expiredCards;
        }

        private static void GetCardPeriod(TypeCard typeCard, Card card)
        {
            if (typeCard.Name.ToLower() == GlobalConstants.YearNamePeriod)
            {
                card.EndEnd = card.StartDate.AddDays(GlobalConstants.DaysOneYear);
            }
            else if (typeCard.Name.ToLower() == GlobalConstants.MonthNamePeriod)
            {
                card.EndEnd = card.StartDate.AddDays(GlobalConstants.DaysOneMonth);
            }
            else
            {
                card.EndEnd = card.StartDate.AddDays(GlobalConstants.DaysOneWeek);
            }
        }
    }
}
