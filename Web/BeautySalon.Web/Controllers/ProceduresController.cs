namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Web.ViewModels.Procedures.InputModels;
    using BeautySalon.Web.ViewModels.Procedures.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class ProceduresController : Controller
    {
        private readonly IProceduresService proceduresService;
        private readonly ICategoriesService categoriesService;

        public ProceduresController(IProceduresService proceduresService, ICategoriesService categoriesService)
        {
            this.proceduresService = proceduresService;
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> ProceduresByCategories()
        {
            var model = new ProceduresByCategoriesViewModel()
            {
                ProceduresByCategories = await this.categoriesService.GetAllAsync<ProceduresByCategoryViewModel>(),
            };

            return this.View(model);
        }

        public async Task<IActionResult> GetProceduresByCategory(string id)
        {
            var caregory = await this.categoriesService.GetByIdAsync(id);

            var model = new AllProceduresByCategoryViewModel()
            {
                CategoryId = id,
                CategoryName = caregory.Name,
                Procedures = await this.proceduresService.GetAllByCategoryAsync<ProcedureViewModel>(id),
            };

            return this.View(model);
        }

        public async Task<IActionResult> GetDetails(string id)
        {
            var model = await this.proceduresService.GetProcedureDetailsAsync<DetailsProcedureViewModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<ActionResult<SearchProcedureCriteriaViewModelModel>> SearchBy([FromBody] SearchProcedureCriteriaInputModel input)
        {
            var category = await this.categoriesService.GetByNameAsync(input.CategoryName);

            if (string.IsNullOrWhiteSpace(input.SkinTypeId) && string.IsNullOrWhiteSpace(input.Criteria))
            {
                return this.RedirectToAction("GetProceduresByCategory", new { id = category.Id });
            }

            var procedures = await this.proceduresService.SearchByAsync<ProcedureViewModel>(input.SkinTypeId, input.Criteria);

            return new SearchProcedureCriteriaViewModelModel { CategoryId = category.Id, Procedures = procedures };
        }
    }
}
