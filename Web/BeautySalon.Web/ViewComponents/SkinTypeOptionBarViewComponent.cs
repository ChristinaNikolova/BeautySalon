namespace BeautySalon.Web.ViewComponents
{
    using System.Threading.Tasks;

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

        public async Task<IViewComponentResult> InvokeAsync(bool hasToAddSecondCriteria)
        {
            var model = new AllSkinTypesViewModel()
            {
                SkinTypes = await this.skinTypesService
                .GetAllAsync<SkinTypeViewModel>(),
                HasToAddSecondCriteria = hasToAddSecondCriteria,
            };

            return this.View(model);
        }
    }
}
