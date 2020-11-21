namespace BeautySalon.Web.ViewModels.Chats.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ClientChatViewModel : IMapFrom<ChatMessage>
    {
        public string SenderUsername { get; set; }

        public string SenderPicture { get; set; }
    }
}
