namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Answers;
    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.Procedures;
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
        private readonly ICategoriesService categoriesService;
        private readonly IProceduresService proceduresService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(
            IAppointmentsService appointmentsService,
            IAnswersService answersService,
            IUsersService usersService,
            ICategoriesService categoriesService,
            IProceduresService proceduresService,
            UserManager<ApplicationUser> userManager)
        {
            this.appointmentsService = appointmentsService;
            this.answersService = answersService;
            this.usersService = usersService;
            this.categoriesService = categoriesService;
            this.proceduresService = proceduresService;
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

        public async Task<IActionResult> MyCard()
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = new MyCardViewModel()
            {
                UserCard = await this.usersService.GetUserCardAsync<UserCardViewModel>(userId),
                SkinCareCategoryId = await this.categoriesService.GetIdByNameAsync(GlobalConstants.CategorySkinName),
                NailsCategoryId = await this.categoriesService.GetIdByNameAsync(GlobalConstants.CategoryNailsName),
                HaircutsProcedureId = await this.proceduresService.GetIdByNameAsync(GlobalConstants.ProcedureHairCutName),
            };

            return this.View(model);
        }
    }
}
