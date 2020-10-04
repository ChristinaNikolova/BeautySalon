namespace BeautySalon.Web.Hubs
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

    [Authorize]
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync(
                "NewMessage");

                // new ChatMessage { User = this.Context.User.Identity.Name, Text = message, });
        }
    }
}
