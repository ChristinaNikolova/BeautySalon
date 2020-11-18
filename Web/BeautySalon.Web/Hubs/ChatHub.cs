namespace BeautySalon.Web.Hubs
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.ChatMessages;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    [Authorize]
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync(
                "NewMessage",
                new ChatMessage { User = this.Context.User.Identity.Name, Content = message, });
        }
    }
}