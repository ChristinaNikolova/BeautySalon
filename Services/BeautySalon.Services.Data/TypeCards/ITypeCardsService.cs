namespace BeautySalon.Services.Data.TypeCards
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITypeCardsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<decimal> GetPriceAsync(string id);
    }
}
