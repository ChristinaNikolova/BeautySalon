namespace BeautySalon.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Models.Enums;
    using BeautySalon.Services.Cloudinary;
    using BeautySalon.Services.Messaging;
    using BeautySalon.Web.Areas.Identity.Pages.Account.InputModels;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
        private readonly ICloudinaryService cloudinaryService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ICloudinaryService cloudinaryService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.cloudinaryService = cloudinaryService;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (this.ModelState.IsValid)
            {
                var existingEmail = await this.userManager.Users
                    .FirstOrDefaultAsync(u => u.Email == this.Input.Email);

                var existingUsername = await this.userManager.Users
                    .FirstOrDefaultAsync(u => u.UserName == this.Input.Username);

                if (existingEmail != null && existingUsername != null)
                {
                    this.TempData["InfoMessage"] = ErrorMessages.UserExists;
                    return this.RedirectToPage("Register");
                }

                if (existingEmail != null)
                {
                    this.TempData["InfoMessage"] = ErrorMessages.EmailExists;
                    return this.RedirectToPage("Register");
                }

                string pictureAsString = await this.cloudinaryService.UploudAsync(this.Input.Picture, this.Input.Username);

                var user = new ApplicationUser()
                {
                    UserName = this.Input.Username,
                    Email = this.Input.Email,
                    FirstName = this.Input.FirstName,
                    LastName = this.Input.LastName,
                    Address = this.Input.Address,
                    PhoneNumber = this.Input.PhoneNumber,
                    Gender = Enum.Parse<Gender>(this.Input.Gender),
                    Birthday = this.Input.Birthday,
                    Picture = pictureAsString,
                };

                var result = await this.userManager.CreateAsync(user, this.Input.Password);

                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = this.Url.Action("ConfirmEmail", "Account", new { code = token, userId = user.Id }, this.Request.Scheme);

                        await this.emailSender.SendEmailAsync(
                            "softuni-beautysalon@abv.bg",
                            "BeautySalon",
                            user.Email,
                            "Email Confirmation",
                            $"<p>{user.UserName}, thank you for your registration at <strong>BeautySalon</strong>! Please, click <a href={confirmationLink}>here</a> to confirm your email.</p>");

                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email });
                    }
                    else
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
