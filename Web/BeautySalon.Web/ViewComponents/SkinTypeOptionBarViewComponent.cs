namespace BeautySalon.Web.ViewComponents
{
    using BeautySalon.Services.Data.SkinTypes;
    using BeautySalon.Web.ViewModels.SkinTypes.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class SkinTypeOptionBarViewComponent : ViewComponent
    {
        private readonly ISkinTypesService skinTypesService;

        public SkinTypeOptionBarViewComponent(ISkinTypesService skinTypesService)
        {
            this.skinTypesService = skinTypesService;
        }

        public IViewComponentResult Invoke()
        {
            var model = new AllSkinTypesViewModel()
            {
                SkinTypes = this.skinTypesService
                .GetAll<SkinTypeViewModel>(),
            };

            return this.View(model);
        }
    }
}
