namespace BeautySalon.Web.Areas.Identity.Pages.Account.Manage
{
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.JobTypes;
    using BeautySalon.Services.Data.Stylists;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Web.Areas.Identity.Pages.Account.InputModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUsersService usersService;
        private readonly IStylistsService stylistsService;
        private readonly ICategoriesService categoriesService;
        private readonly IJobTypesService jobTypesService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUsersService usersService,
            IStylistsService stylistsService,
            ICategoriesService categoriesService1,
            IJobTypesService jobTypesService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.usersService = usersService;
            this.stylistsService = stylistsService;
            this.categoriesService = categoriesService1;
            this.jobTypesService = jobTypesService;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public IFormFile Picture { get; set; }

        public string SkinType { get; set; }

        public bool IsSkinSensitive { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public UpdateProfileInputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            await this.usersService.UpdateUserProfileAsync(user.Id, user.UserName, this.Input.FirstName, this.Input.LastName, this.Input.Address, this.Input.PhoneNumber, this.Input.Gender, this.Input.SkinType, this.Input.IsSkinSensitive, this.Input.Picture);

            //if (this.User.IsInRole(GlobalConstants.StylistRoleName))
            //{
            //    await this.stylistsService.UpdateStylistProfileAsync(user.Id, this.Input.Category, this.Input.JobType, this.Input.Description);
            //}

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await this.userManager.GetUserNameAsync(user);
            var email = await this.userManager.GetEmailAsync(user);
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);

            this.Username = userName;
            this.Email = email;

            var skinType = await this.usersService.GetUserSkinTypeByIdAsync(user.SkinTypeId);

            this.Input = new UpdateProfileInputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Gender = user.Gender.ToString(),
                IsSkinSensitive = user.IsSkinSensitive.Value ? "Yes" : "No",
            };

            if (skinType != null)
            {
                this.Input.SkinType = skinType.Name;
            }

            if (this.User.IsInRole(GlobalConstants.StylistRoleName))
            {
                var category = await this.categoriesService.GetByIdAsync(user.CategoryId);
                var jobType = await this.jobTypesService.GetByIdAsync(user.JobTypeId);

                this.Input.Category = category.Name;
                this.Input.JobType = jobType.Name;
                this.Input.Description = user.Description;
            }
        }
    }
}
