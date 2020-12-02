namespace BeautySalon.Services.Data.TypeCards
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class TypeCardsService : ITypeCardsService
    {
        private readonly IRepository<TypeCard> typeCardsRepository;

        public TypeCardsService(IRepository<TypeCard> typeCardsRepository)
        {
            this.typeCardsRepository = typeCardsRepository;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var typeCards = await this.typeCardsRepository
                .All()
                .OrderByDescending(tc => tc.Price)
                .To<T>()
                .ToListAsync();

            return typeCards;
        }

        public async Task<decimal> GetPriceAsync(string id)
        {
            var price = await this.typeCardsRepository
                .All()
                .Where(tc => tc.Id == id)
                .Select(tc => tc.Price)
                .FirstOrDefaultAsync();

            return price;
        }
    }
}
