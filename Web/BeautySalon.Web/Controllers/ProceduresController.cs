﻿namespace BeautySalon.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Web.ViewModels.Procedures.InputModels;
    using BeautySalon.Web.ViewModels.Procedures.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class ProceduresController : Controller
    {
        private const int ProceduresPerPage = 12;

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

        public async Task<IActionResult> GetProceduresByCategory(string id, int currentPage = 1)
        {
            var proceduresCount = await this.proceduresService.GetTotalCountProceduresByCategoryAsync(id);

            var pageCount = (int)Math.Ceiling((double)proceduresCount / ProceduresPerPage);

            var procedures = await this.proceduresService
                .GetAllByCategoryAsync<ProcedureViewModel>(id, ProceduresPerPage, (currentPage - 1) * ProceduresPerPage);

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
