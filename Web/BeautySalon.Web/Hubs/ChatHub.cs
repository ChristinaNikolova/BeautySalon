namespace BeautySalon.Web.Hubs
{
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.ChatMessages;
    using BeautySalon.Web.ViewModels.Chats.InputModels;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IChatsService chatService;

        public ChatHub(UserManager<ApplicationUser> userManager, IChatsService chatService)
        {
            this.userManager = userManager;
            this.chatService = chatService;
        }

        public async Task Send(SendChatMessageInputModel input)
        {
            var sanitizer = new HtmlSanitizer();
            var message = sanitizer.Sanitize(input.ChatMessage);

            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var sender = await this.userManager.Users.FirstAsync(u => u.UserName == input.SenderUsername);
            var receiver = await this.userManager.Users.FirstAsync(u => u.UserName == input.ReceiverUsername);

            await this.chatService.SendMessageAsync(input.ChatMessage, sender, receiver, input.GroupName);

            await this.Clients
                .User(receiver.Id)
                .SendAsync("ReceiveMessage", message, sender.UserName, sender.Picture, input.GroupName);

            await this.Clients
                .Caller
                .SendAsync("SendMessage", message, sender.UserName, sender.Picture);
        }

        public async Task CreateGroup(string senderUsername, string receiverUsername, string groupName)
        {
            var sender = await this.userManager.FindByNameAsync(senderUsername);
            var receiver = await this.userManager.FindByNameAsync(receiverUsername);

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            await this.chatService.CreateUsersGroup(sender, receiver, groupName);
        }
    }
}
