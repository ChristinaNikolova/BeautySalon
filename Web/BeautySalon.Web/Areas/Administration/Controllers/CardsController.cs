namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Cards;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Web.ViewModels.Administration.Cards.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class CardsController : AdministrationController
    {
        private readonly ICardsService cardsService;
        private readonly IUsersService usersService;

        public CardsController(
            ICardsService cardsService,
            IUsersService usersService)
        {
            this.cardsService = cardsService;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AllCardsViewModel()
            {
                ActiveCards = await this.cardsService.GetActiveCardsAsync<CardViewModel>(),
                ExpiredCards = await this.cardsService.GetExpiredCardsAsync<CardViewModel>(),
            };

            await this.GetClientUsernameAsync(model);

            return this.View(model);
        }

        private async Task GetClientUsernameAsync(AllCardsViewModel model)
        {
            for (int i = 0; i < model.ActiveCards.Count(); i++)
            {
                var currentCardModel = model.ActiveCards.ToList()[i];

                currentCardModel.ClientUsername = await this.usersService.GetUsernameByIdAsync(currentCardModel.ClientId);
            }

            for (int i = 0; i < model.ExpiredCards.Count(); i++)
            {
                var currentCardModel = model.ExpiredCards.ToList()[i];

                currentCardModel.ClientUsername = await this.usersService.GetUsernameByIdAsync(currentCardModel.ClientId);
            }
        }
    }
}
