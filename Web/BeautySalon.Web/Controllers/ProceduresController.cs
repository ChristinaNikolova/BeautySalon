namespace BeautySalon.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Web.ViewModels.Appointments.ViewModels;
    using BeautySalon.Web.ViewModels.Procedures.InputModels;
    using BeautySalon.Web.ViewModels.Procedures.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ProceduresController : BaseController
    {
        private readonly IProceduresService proceduresService;
        private readonly ICategoriesService categoriesService;
        private readonly IAppointmentsService appointmentsService;

        public ProceduresController(
            IProceduresService proceduresService,
            ICategoriesService categoriesService,
            IAppointmentsService appointmentsService)
        {
            this.proceduresService = proceduresService;
            this.categoriesService = categoriesService;
            this.appointmentsService = appointmentsService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> ProceduresByCategories()
        {
            var model = new ProceduresByCategoriesViewModel()
            {
                ProceduresByCategories = await this.categoriesService.GetAllAsync<ProceduresByCategoryViewModel>(),
            };

            return this.View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetProceduresByCategory(string id, int currentPage = 1)
        {
            var proceduresCount = await this.proceduresService.GetTotalCountProceduresByCategoryAsync(id);

            var pageCount = (int)Math.Ceiling((double)proceduresCount / GlobalConstants.ProceduresPerPage);

            var procedures = await this.proceduresService
                .GetAllByCategoryAsync<ProcedureViewModel>(id, GlobalConstants.ProceduresPerPage, (currentPage - 1) * GlobalConstants.ProceduresPerPage);

            var caregory = await this.categoriesService.GetByIdAsync(id);

            var model = new AllProceduresByCategoryViewModel()
            {
                CategoryId = id,
                CategoryName = caregory.Name,
                Procedures = procedures,
                CurrentPage = currentPage,
                PagesCount = pageCount,
            };

            return this.View(model);
        }

        public async Task<IActionResult> GetDetails(string id)
        {
            var model = await this.proceduresService.GetProcedureDetailsAsync<DetailsProcedureViewModel>(id);

            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<SearchProcedureCriteriaViewModelModel>> SearchBy([FromBody] SearchProcedureCriteriaInputModel input)
        {
            var category = await this.categoriesService.GetByNameAsync(input.CategoryName);

            if (string.IsNullOrWhiteSpace(input.SkinTypeId) && string.IsNullOrWhiteSpace(input.Criteria))
            {
                return this.RedirectToAction(nameof(this.GetProceduresByCategory), new { id = category.Id });
            }

            var procedures = await this.proceduresService.SearchByAsync<ProcedureViewModel>(input.SkinTypeId, input.Criteria);

            return new SearchProcedureCriteriaViewModelModel { CategoryId = category.Id, Procedures = procedures };
        }

        [HttpPost]
        public async Task<ActionResult<AllProcedureNamesViewModel>> GetProceduresByStylist([FromBody] string stylistId)
        {
            var procedureNames = await this.proceduresService.GetProceduresByStylistAsync<ProcedureNameViewModel>(stylistId);

            return new AllProcedureNamesViewModel { ProcedureNames = procedureNames };
        }

        public async Task<ActionResult<AllProcedureNamesViewModel>> SmartSearchProcedures([FromBody] ProcedureSmartSeachInputModel input)
        {
            var procedureNames = await this.proceduresService.GetSmartSearchProceduresAsync<ProcedureNameViewModel>(input.ClientSkinTypeId, input.IsSkinSensitive, input.StylistId);

            return new AllProcedureNamesViewModel { ProcedureNames = procedureNames };
        }

        public async Task<ActionResult> Review(string appointmentId)
        {
            var model = new ReviewProcedureInputModel()
            {
                Appointment = await this.appointmentsService.GetByIdAsync<AppointmentViewModel>(appointmentId),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Review(ReviewProcedureInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Appointment = await
                    this.appointmentsService.GetByIdAsync<AppointmentViewModel>(input.AppoitmentId);

                return this.View(input);
            }

            await this.proceduresService.AddProcedureReviewsAsync(input.AppoitmentId, input.Content, input.Points);

            this.TempData["InfoMessage"] = GlobalMessages.SuccessCreateMessage;

            return this.RedirectToAction("GetUsersHistory", "Appointments", new { Id = input.AppoitmentId });
        }
    }
}
