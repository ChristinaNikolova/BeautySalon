namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.ChatMessages;
    using BeautySalon.Web.ViewModels.Chats.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ChatsController : BaseController
    {
        private readonly IChatsService chatsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatsController(
            IChatsService chatsService,
            UserManager<ApplicationUser> userManager)
        {
            this.chatsService = chatsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var admin = await this.userManager.FindByNameAsync(GlobalConstants.AdminName);

            var model = new IndexViewModel()
            {
                AdminId = admin.Id,
                Users = await this.chatsService.GetWaitingForAnswerClientNamesAsync<ClientChatViewModel>(),
            };

            return this.View(model);
        }

        public async Task<IActionResult> StartChat(string id)
        {
            var client = await this.userManager.GetUserAsync(this.User);
            var receiver = await this.userManager.FindByIdAsync(id);

            var groupName = string.Empty;

            if (client.UserName != GlobalConstants.AdminName)
            {
                groupName = client.UserName;
            }
            else
            {
                groupName = receiver.UserName;
            }

            var groupId = await this.chatsService.GetGroupIdAsync(groupName);

            var model = new StartChatViewModel()
            {
                CurrentUserUsername = this.userManager.GetUserName(this.User),
                SenderUsername = client.UserName,
                ReceiverUsername = receiver.UserName,
                GroupName = groupName,
                ChatMessages = await this.chatsService.GetOldMessagesAsync<ChatMessageViewModel>(groupId),
            };

            return this.View(model);
        }
    }
}
