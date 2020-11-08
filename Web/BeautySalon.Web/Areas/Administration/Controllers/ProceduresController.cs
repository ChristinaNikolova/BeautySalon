namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Services.Data.SkinTypes;
    using BeautySalon.Web.ViewModels.Administration.Procedures.InputModels;
    using BeautySalon.Web.ViewModels.Administration.Procedures.ViewModels;
    using BeautySalon.Web.ViewModels.Procedures.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class ProceduresController : AdministrationController
    {
        private readonly IProceduresService proceduresService;
        private readonly ICategoriesService categoriesService;
        private readonly ISkinTypesService skinTypesService;

        public ProceduresController(IProceduresService proceduresService, ICategoriesService categoriesService, ISkinTypesService skinTypesService)
        {
            this.proceduresService = proceduresService;
            this.categoriesService = categoriesService;
            this.skinTypesService = skinTypesService;
        }

        public async Task<IActionResult> GetAll()
        {
            var model = new AllProceduresAdministrationViewModel()
            {
                Procedures = await this.proceduresService.GetAllAdministrationAsync<ProcedureViewModel>(),
            };

            return this.View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new AddProcedureInputModel()
            {
                Categories = await this.categoriesService.GetAllAsSelectListItemAsync(),
                SkinTypes = await this.skinTypesService.GetAllAsSelectListItemAsync(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddProcedureInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.categoriesService.GetAllAsSelectListItemAsync();
                input.SkinTypes = await this.skinTypesService.GetAllAsSelectListItemAsync();

                return this.View(input);
            }

            await this.proceduresService.CreateAsync(input.Name, input.Description, input.Price, input.CategoryId, input.SkinTypeId, input.IsSensitive);

            //redirect to Update!
            return this.RedirectToAction(nameof(this.GetAll));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.proceduresService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.GetAll));
        }
    }
}
