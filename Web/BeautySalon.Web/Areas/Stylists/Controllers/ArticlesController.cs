namespace BeautySalon.Web.Areas.Stylists.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Articles;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Web.ViewModels.StylistsArea.Articles.InputModels;
    using BeautySalon.Web.ViewModels.StylistsArea.Articles.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : StylistsController
    {
        private readonly IArticlesService articlesService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ArticlesController(IArticlesService articlesService, ICategoriesService categoriesService, UserManager<ApplicationUser> userManager)
        {
            this.articlesService = articlesService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetAllForStylist()
        {
            var stylistId = this.userManager.GetUserId(this.User);

            var model = new AllArticleStylistAreaViewModel()
            {
                Articles = await this.articlesService
                .GetAllForStylistAsync<ArticleStylistAreaViewModel>(stylistId),
            };

            return this.View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new CreateArticleInputModel()
            {
                Categories = await this.categoriesService.GetAllAsSelectListItemAsync(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.categoriesService.GetAllAsSelectListItemAsync();

                return this.View(input);
            }

            var stylistId = this.userManager.GetUserId(this.User);

            string articleId = await this.articlesService.CreateAsync(input.Title, input.Content, input.CategoryId, input.Picture, stylistId);

            return this.RedirectToAction(nameof(this.Update), new { id = articleId });
        }

        public async Task<IActionResult> Update(string id)
        {
            var model = await this.articlesService.GetDataForUpdateAsync<UpdateArticleInputModel>(id);

            model.Categories = await this.categoriesService.GetAllAsSelectListItemAsync();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateArticleInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.categoriesService.GetAllAsSelectListItemAsync();
                input.Picture = await this.articlesService.GetPictureUrlAsync(input.Id);

                return this.View(input);
            }

            await this.articlesService.UpdateAsync(input.Title, input.Content, input.CategoryId, input.NewPicture, input.Id);

            return this.RedirectToAction(nameof(this.GetAllForStylist));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.articlesService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.GetAllForStylist));
        }
    }
}
