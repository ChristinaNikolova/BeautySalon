namespace BeautySalon.Web.Hubs
{
    using BeautySalon.Services.Data.ChatMessages;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatMessagesService chatMessagesService;

        public ChatHub(IChatMessagesService chatMessagesService)
        {
            this.chatMessagesService = chatMessagesService;
        }

        // public async Task Send(SendMessageInputModel inputModel)
        // {
        //    var newChatMessage = this.chatMessagesService.CreateAsync();
        // }
    }
}


// var caller = this.userManager
//    .Users
//    .First(x => x.Id == inputModel.CallerId)
//    .ProfilePictureUrl;

// await this.Clients
//    .User(inputModel.UserId)
//    .SendAsync("RecieveMessage", message, caller);

// await this.Clients
//    .Caller
//    .SendAsync("SendMessage", message);
