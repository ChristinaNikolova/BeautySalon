namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Common;
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

        public ProceduresController(
            IProceduresService proceduresService,
            ICategoriesService categoriesService,
            ISkinTypesService skinTypesService,
            ISkinProblemsService skinProblemsService,
            IProductsService productsService)
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

            this.TempData["InfoMessage"] = GlobalMessages.SuccessCreateMessage;

            return this.RedirectToAction(nameof(this.GetAll));
        }

        public async Task<IActionResult> Update(string id)
        {
            var model = await this.proceduresService.GetProcedureDetailsAsync<UpdateProcedureInputModel>(id);

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

            this.TempData["InfoMessage"] = GlobalMessages.SuccessUpdateMessage;

            return this.RedirectToAction(nameof(this.GetAll));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.proceduresService.DeleteAsync(id);

            this.TempData["InfoMessage"] = GlobalMessages.SuccessDeleteMessage;

            return this.RedirectToAction(nameof(this.GetAll));
        }

        public async Task<IActionResult> ManageProducts(string id)
        {
            var model = await this.proceduresService
                .GetProcedureDetailsAsync<ManageProcedureProductsViewModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductProcedureInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.ManageProducts), new { input.Id });
            }

            var productId = await this.productsService.GetProductIdByNameAsync(input.ProductName);

            if (productId == null)
            {
                return this.RedirectToAction(nameof(this.ManageProducts), new { input.Id });
            }

            var isSuccess = await this.proceduresService.AddProductToProcedureAsync(input.Id, productId);

            if (isSuccess)
            {
                this.TempData["InfoMessage"] = GlobalMessages.SuccessCreateMessage;
            }

            return this.RedirectToAction(nameof(this.ManageProducts), new { input.Id });
        }

        [HttpPost]
        public async Task<ActionResult<ManageProcedureProductsViewModel>> DeleteProcedureProduct([FromBody] DeleteProductProcedureInputModel input)
        {
            await this.proceduresService.RemoveProductAsync(input.ProductId, input.ProcedureId);

            var procedureProducts = await this.proceduresService
                .GetProcedureDetailsAsync<ManageProcedureProductsViewModel>(input.ProcedureId);

            this.TempData["InfoMessage"] = GlobalMessages.SuccessDeleteMessage;

            return procedureProducts;
        }
    }
}
