using System.Threading.Tasks;
using BeautySalon.Common;
using BeautySalon.Data.Models;
using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ChatHub(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        //public async Task Send(string message, string picture)
        //{
        //    await this.Clients.All.SendAsync(
        //        "NewMessage",
        //        new ChatMessage { User = this.Context.User.Identity.Name, Content = message, Picture = picture });
        //}

        public async Task Send(SendMessageInputModel input)
        {
            var sanitizer = new HtmlSanitizer();
            var message = sanitizer.Sanitize(input.ChatMessage);

            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var connId = this.Context.ConnectionId;
            var clients = this.Clients.Caller;
            ;
            //var testUser = await this.userManager.FindByIdAsync(input.UserId);
            var senderUsername = this.Context.User.Identity.Name;

            var sender = await this.userManager.Users.FirstAsync(u => u.UserName == senderUsername);
            ;
            //if (input.Id != input.UserId)
            //{
            //    sender = await this.userManager.Users.FirstAsync(u => u.Id == input.AdminId);
            //}
            ;
            //var receiver = await this.userManager.Users.FirstAsync(u => u.Id == input.ReceiverId);


            //await this.Clients
            //    .User(receiver.Id)
            //    .SendAsync("RecieveMessage", message, sender.UserName, sender.Picture);

            //await this.Clients
            //    .Caller
            //    .SendAsync("SendMessage", message, sender.UserName, sender.Picture);
        }
    }

    public class SendMessageInputModel
    {
        public string ChatMessage { get; set; }

        public string AdminId { get; set; }

        public string UserId { get; set; }
    }
}