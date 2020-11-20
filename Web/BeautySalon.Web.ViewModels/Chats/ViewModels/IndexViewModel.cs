namespace BeautySalon.Web.ViewModels.Chats.ViewModels
{
    using System.Collections.Generic;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class IndexViewModel
    {
        public string AdminUsername { get; set; }

        public IEnumerable<ClientChatViewModel> Clients { get; set; }
    }

    public class ClientChatViewModel : IMapFrom<ChatMessage>
    {
        public string SenderUsername { get; set; }

        public string SenderPicture { get; set; }
    }
}
