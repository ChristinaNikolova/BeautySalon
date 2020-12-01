namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;
    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Cards;
    using BeautySalon.Services.Paypal;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class PaypalController : BaseController
    {
        private readonly IPaypalService paypalService;
        private readonly ICardsService cardsService;
        private readonly UserManager<ApplicationUser> userManager;

        public PaypalController(
            IPaypalService paypalService,
            ICardsService cardsService,
            UserManager<ApplicationUser> userManager)
        {
            this.paypalService = paypalService;
            this.cardsService = cardsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> CreatePayment([FromQuery] int price)
        {
            var result = await this.paypalService.CreatePayment(price);

            foreach (var link in result.links)
            {
                if (link.rel.Equals("approval_url"))
                {
                    return this.Redirect(link.href);
                }
            }

            return this.NotFound();
        }

        public async Task<IActionResult> SuccessedPayment(string paymentId, string token, string payerId, [FromQuery] int price)
        {
            await this.paypalService.ExecutePayment(payerId, paymentId, token);

            var userId = this.userManager.GetUserId(this.User);

            await this.cardsService.CreateCardAsync(userId, price);

            this.TempData["InfoMessage"] = GlobalMessages.SuccessBoughtCard;

            return this.Redirect("/Users/MyCard");
        }
    }
}
