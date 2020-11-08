namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.JobTypes;
    using BeautySalon.Services.Data.Stylists;
    using BeautySalon.Web.ViewModels.Administration.Stylists.InputModels;
    using BeautySalon.Web.ViewModels.Administration.Stylists.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class StylistsController : AdministrationController
    {
        private readonly IStylistsService stylistsService;
        private readonly ICategoriesService categoriesService;
        private readonly IJobTypesService jobTypesService;

        public StylistsController(IStylistsService stylistsService, ICategoriesService categoriesService, IJobTypesService jobTypesService)
        {
            this.stylistsService = stylistsService;
            this.categoriesService = categoriesService;
            this.jobTypesService = jobTypesService;
        }

        public async Task<IActionResult> GetAll()
        {
            var model = new AllStylistsAdministrationViewModel()
            {
                Stylists = await this.stylistsService.GetAllAdministrationAsync<StylistAdministrationViewModel>(),
            };

            return this.View(model);
        }

        public async Task<IActionResult> Update(string id)
        {
            var model = await this.stylistsService.GetStylistDataForUpdateAsync<UpdateStylistInputModel>(id);

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

                return this.View(input);
            }

            await this.stylistsService.UpdateStylistProfileAsync(input.Id, input.FirstName, input.LastName, input.PhoneNumber, input.CategoryId, input.JobTypeId, input.Description, input.NewPicture);

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

            return this.RedirectToAction(nameof(this.Update), new { Id = stylistId });
        }
    }
}
