namespace BeautySalon.Web.ViewComponents
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Web.ViewModels.Categories.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class CategoryOptionBarViewComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;

        public CategoryOptionBarViewComponent(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool hasToAddSecondCriteria)
        {
            var model = new AllCategoriesViewModel()
            {
                Categories = await this.categoriesService
                .GetAllAsync<CategoryViewModel>(),
                HasToAddSecondCriteria = hasToAddSecondCriteria,
            };

            return this.View(model);
        }
    }
}
