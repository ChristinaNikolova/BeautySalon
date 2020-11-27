namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Answers;
    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Web.ViewModels.Appointments.ViewModels;
    using BeautySalon.Web.ViewModels.Users.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        private readonly IAppointmentsService appointmentsService;
        private readonly IAnswersService answersService;
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(
            IAppointmentsService appointmentsService,
            IAnswersService answersService,
            IUsersService usersService,
            UserManager<ApplicationUser> userManager)
        {
            this.appointmentsService = appointmentsService;
            this.answersService = answersService;
            this.usersService = usersService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = new IndexUserViewModel()
            {
                Appoitments = await this.appointmentsService.GetClientsUpcomingAppointmentsAsync<BaseAppoitmentViewModel>(userId),
                IsNewAnswer = await this.answersService.CheckNewAnswerAsync(userId),
                HasToReview = await this.appointmentsService.CheckPastProceduresAsync(userId),
            };

            return this.View(model);
        }

        public async Task<IActionResult> GetUsersSkinInfo()
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = await
                this.usersService.GetUserDataAsync<UsersSkinTypeInfoViewModel>(userId);

            return this.View(model);
        }
    }
}
