namespace BeautySalon.Services.Data.Cards
{
    using System.Threading.Tasks;

    public interface ICardsService
    {
        Task CreateCardAsync(string userId, int price);
    }
}
