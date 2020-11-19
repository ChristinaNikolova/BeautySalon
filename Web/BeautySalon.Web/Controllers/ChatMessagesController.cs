namespace BeautySalon.Web.Controllers
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.ChatMessages;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ChatMessagesController : BaseController
    {
        private readonly IChatService chatMessagesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatMessagesController(
            IChatService chatMessagesService,
            UserManager<ApplicationUser> userManager)
        {
            this.chatMessagesService = chatMessagesService;
            this.userManager = userManager;
        }

        public IActionResult Chat()
        {
            return this.View();
        }
    }
}
