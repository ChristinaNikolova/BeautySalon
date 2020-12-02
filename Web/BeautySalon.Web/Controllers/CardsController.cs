namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Services.Data.TypeCards;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Web.ViewModels.TypeCards.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CardsController : BaseController
    {
        private readonly ITypeCardsService typeCardsService;
        private readonly IUsersService usersService;
        private readonly ICategoriesService categoriesService;
        private readonly IProceduresService proceduresService;
        private readonly UserManager<ApplicationUser> userManager;

        public CardsController(
            ITypeCardsService typeCardsService,
            IUsersService usersService,
            ICategoriesService categoriesService,
            IProceduresService proceduresService,
            UserManager<ApplicationUser> userManager)
        {
            this.typeCardsService = typeCardsService;
            this.usersService = usersService;
            this.categoriesService = categoriesService;
            this.proceduresService = proceduresService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetAllTypeCards()
        {
            var userId = this.userManager.GetUserId(this.User);
            var model = new AllTypeCardsViewModel()
            {
                TypeCards = await this.typeCardsService.GetAllAsync<TypeCardViewModel>(),
                HasCard = await this.usersService.HasSubscriptionCardAsync(userId),
                SkinCareCategoryId = await this.categoriesService.GetIdByNameAsync(GlobalConstants.CategorySkinName),
                NailsCategoryId = await this.categoriesService.GetIdByNameAsync(GlobalConstants.CategoryNailsName),
                HaircutsProcedureId = await this.proceduresService.GetIdByNameAsync(GlobalConstants.ProcedureHairCutName),
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
