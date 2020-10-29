namespace BeautySalon.Web.ViewComponents
{
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

        public IViewComponentResult Invoke()
        {
            var model = new AllCategoriesViewModel()
            {
                Categories = this.categoriesService
                .GetAll<CategoryViewModel>(),
            };

            return this.View(model);
        }
    }
}
