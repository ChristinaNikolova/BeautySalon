namespace BeautySalon.Web.ViewComponents
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Web.ViewModels.Procedures.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class BestSkinProceduresForUserViewComponent : ViewComponent
    {
        private readonly IProceduresService proceduresService;

        public BestSkinProceduresForUserViewComponent(IProceduresService proceduresService)
        {
            this.proceduresService = proceduresService;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool isSkinSensitive, string skinTypeId)
        {
            if (skinTypeId == null)
            {
                return null;
            }

            var model = new AllProcedurePrimaryViewModel()
            {
                Procedures = await this.proceduresService
                .GetPerfectProceduresForSkinTypeAsync<ProcedurePrimaryViewModel>(isSkinSensitive, skinTypeId),
            };

            return this.View(model);
        }
    }
}
