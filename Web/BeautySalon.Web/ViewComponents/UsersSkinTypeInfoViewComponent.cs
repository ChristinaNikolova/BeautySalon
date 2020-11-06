namespace BeautySalon.Web.ViewComponents
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Users;
    using BeautySalon.Web.ViewModels.Users.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class UsersSkinTypeInfoViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public UsersSkinTypeInfoViewComponent(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var model = await this.usersService.GetUserSkinDataAsync<UsersSkinTypeInfoViewModel>(userId);

            return this.View(model);
        }
    }
}
