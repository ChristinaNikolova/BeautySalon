namespace BeautySalon.Services.Data.Cards
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICardsService
    {
        Task CreateCardAsync(string userId, int price);

        Task<IEnumerable<T>> GetActiveCardsAsync<T>();

        Task<IEnumerable<T>> GetExpiredCardsAsync<T>();

        Task ChangeCardCounterAsync(string userId, decimal price);
    }
}
