namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.TypeCards;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Services.Paypal;
    using BeautySalon.Web.ViewModels.TypeCards.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CardsController : BaseController
    {
        private readonly ITypeCardsService typeCardsService;
        private readonly IUsersService usersService;
        private readonly IPaypalService paypalService;
        private readonly UserManager<ApplicationUser> userManager;

        public CardsController(
            ITypeCardsService typeCardsService,
            IUsersService usersService,
            IPaypalService paypalService,
            UserManager<ApplicationUser> userManager)
        {
            this.typeCardsService = typeCardsService;
            this.usersService = usersService;
            this.paypalService = paypalService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetAllTypeCards()
        {
            var userId = this.userManager.GetUserId(this.User);
            var model = new AllTypeCardsViewModel()
            {
                TypeCards = await this.typeCardsService.GetAllAsync<TypeCardViewModel>(),
                HasCard = await this.usersService.HasSubscriptionCardAsync(userId),
            };

            return this.View(model);
        }

        public async Task<IActionResult> Buy(string id)
        {
            var price = await this.typeCardsService.GetPriceAsync(id);

            return this.Redirect($"/Paypal/CreatePayment?price={price}");
        }
    }
}
