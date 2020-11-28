namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.JobTypes;
    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Services.Data.Stylists;
    using BeautySalon.Web.ViewModels.Administration.Stylists.InputModels;
    using BeautySalon.Web.ViewModels.Administration.Stylists.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class StylistsController : AdministrationController
    {
        private readonly IStylistsService stylistsService;
        private readonly ICategoriesService categoriesService;
        private readonly IJobTypesService jobTypesService;
        private readonly IProceduresService proceduresService;
        private readonly UserManager<ApplicationUser> userManager;

        public StylistsController(
            IStylistsService stylistsService,
            ICategoriesService categoriesService,
            IJobTypesService jobTypesService,
            IProceduresService proceduresService,
            UserManager<ApplicationUser> userManager)
        {
            this.stylistsService = stylistsService;
            this.categoriesService = categoriesService;
            this.jobTypesService = jobTypesService;
            this.proceduresService = proceduresService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetAll()
        {
            var model = new AllStylistsAdministrationViewModel()
            {
                Stylists = await this.stylistsService.GetAllAsync<StylistAdministrationViewModel>(),
            };

            return this.View(model);
        }

        public async Task<IActionResult> Update(string id)
        {
            var model = await this.stylistsService.GetStylistDetailsAsync<UpdateStylistInputModel>(id);

            model.Categories = await this.categoriesService.GetAllAsSelectListItemAsync();
            model.JobTypes = await this.jobTypesService.GetAllAsSelectListItemAsync();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateStylistInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.categoriesService.GetAllAsSelectListItemAsync();
                input.JobTypes = await this.jobTypesService.GetAllAsSelectListItemAsync();
                input.Picture = await this.stylistsService.GetPictureUrlAsync(input.Id);

                return this.View(input);
            }

            await this.stylistsService.UpdateStylistProfileAsync(input.Id, input.FirstName, input.LastName, input.PhoneNumber, input.CategoryId, input.JobTypeId, input.Description, input.NewPicture);

            this.TempData["InfoMessage"] = GlobalMessages.SuccessUpdateMessage;

            return this.RedirectToAction(nameof(this.GetAll));
        }

        [HttpPost]
        public async Task<IActionResult> Add(string stylistEmail)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect(nameof(this.GetAll));
            }

            var stylistId = await this.stylistsService.AddRoleStylistAsync(stylistEmail);

            if (stylistId == null)
            {
                return this.RedirectToAction(nameof(this.GetAll));
            }

            this.TempData["InfoMessage"] = GlobalMessages.SuccessCreateMessage;

            return this.RedirectToAction(nameof(this.GetAll));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var stylist = await this.stylistsService.GetByIdAsync(id);

            await this.userManager.RemoveFromRoleAsync(stylist, GlobalConstants.StylistRoleName);
            await this.stylistsService.RemoveAllProceduresAsync(id);

            this.TempData["InfoMessage"] = GlobalMessages.SuccessDeleteMessage;

            return this.RedirectToAction(nameof(this.GetAll));
        }

        public async Task<IActionResult> ManageProcedures(string id)
        {
            var model = await this.stylistsService.GetStylistDetailsAsync<ManageStylistProceduresViewModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProcedure(AddProcedureStylistInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.ManageProcedures), new { input.Id });
            }

            var procedureId = await this.proceduresService.GetProcedureIdByNameAsync(input.ProcedureName);

            if (procedureId == null)
            {
                return this.RedirectToAction(nameof(this.ManageProcedures), new { input.Id });
            }

            var isSuccess = await this.stylistsService.AddProcedureToStylistAsync(input.Id, procedureId);

            if (!isSuccess)
            {
                return this.RedirectToAction(nameof(this.ManageProcedures), new { input.Id });
            }

            this.TempData["InfoMessage"] = GlobalMessages.SuccessCreateMessage;

            return this.RedirectToAction(nameof(this.ManageProcedures), new { input.Id });
        }

        [HttpPost]
        public async Task<ActionResult<ManageStylistProceduresViewModel>> DeleteStylistProcedure([FromBody] DeleteProcedureStylistInputModel input)
        {
            await this.stylistsService.RemoveProcedureAsync(input.StylistId, input.ProcedureId);

            var stylistProcedures = await this.stylistsService.GetStylistDetailsAsync<ManageStylistProceduresViewModel>(input.StylistId);

            this.TempData["InfoMessage"] = GlobalMessages.SuccessDeleteMessage;

            return stylistProcedures;
        }
    }
}
