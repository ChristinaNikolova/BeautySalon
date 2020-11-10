namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Services.Data.Products;
    using BeautySalon.Services.Data.SkinProblems;
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
        private readonly ISkinProblemsService skinProblemsService;
        private readonly IProductsService productsService;

        public ProceduresController(IProceduresService proceduresService, ICategoriesService categoriesService, ISkinTypesService skinTypesService, ISkinProblemsService skinProblemsService, IProductsService productsService)
        {
            this.proceduresService = proceduresService;
            this.categoriesService = categoriesService;
            this.skinTypesService = skinTypesService;
            this.skinProblemsService = skinProblemsService;
            this.productsService = productsService;
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
                SkinProblems = await this.skinProblemsService.GetAllAsSelectListItemAsync(),
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
                input.SkinProblems = await this.skinProblemsService.GetAllAsSelectListItemAsync();

                return this.View(input);
            }

            var id = await this.proceduresService.CreateAsync(input.Name, input.Description, input.Price, input.CategoryId, input.SkinTypeId, input.IsSensitive, input.SkinProblems);

            return this.RedirectToAction(nameof(this.Update), new { Id = id });
        }

        public async Task<IActionResult> Update(string id)
        {
            var model = await this.proceduresService.GetProcedureDataForUpdateAsync<UpdateProcedureInputModel>(id);

            model.Categories = await this.categoriesService.GetAllAsSelectListItemAsync();
            model.SkinTypes = await this.skinTypesService.GetAllAsSelectListItemAsync();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProcedureInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.categoriesService.GetAllAsSelectListItemAsync();
                input.SkinTypes = await this.skinTypesService.GetAllAsSelectListItemAsync();

                return this.View(input);
            }

            await this.proceduresService.UpdateAsync(input.Id, input.Name, input.Description, input.Price, input.CategoryId, input.SkinTypeId, input.IsSensitive);

            return this.RedirectToAction(nameof(this.GetAll));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.proceduresService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.GetAll));
        }

        public async Task<IActionResult> ManageProducts(string id)
        {
            var model = await this.proceduresService
                .GetProcedureProductsAdministrationAsync<ManageProcedureProductsViewModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Test22 input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.ManageProducts), new { Id = input.Id });
            }

            var productId = await this.productsService.GetProductIdByNameAsync(input.ProductName);

            if (productId == null)
            {
                return this.RedirectToAction(nameof(this.ManageProducts), new { Id = input.Id });
            }

            var isSuccess = await this.proceduresService.AddProductToProcedureAsync(input.Id, productId);

            if (!isSuccess)
            {
                return this.RedirectToAction(nameof(this.ManageProducts), new { Id = input.Id });
            }

            return this.RedirectToAction(nameof(this.Update), new { Id = input.Id });
        }

        [HttpPost]
        public async Task<ActionResult<ManageProcedureProductsViewModel>> DeleteProcedureProduct([FromBody] Test11 input)
        {
            await this.proceduresService.RemoveProductAsync(input.ProductId, input.ProcedureId);

            var procedureProducts = await this.proceduresService.GetProcedureProductsAdministrationAsync<ManageProcedureProductsViewModel>(input.ProcedureId);

            return procedureProducts;
        }
    }

    public class Test22
    {
        public string ProductName { get; set; }

        public string Id { get; set; }
    }

    public class Test11
    {
        public string ProductId { get; set; }

        public string ProcedureId { get; set; }
    }
}
