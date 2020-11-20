namespace BeautySalon.Web.ViewModels.Chats.ViewModels
{
    using System.Collections.Generic;

    public class StartChatViewModel
    {
        public string SenderUsername { get; set; }

        public string ReceiverUsername { get; set; }

        public IEnumerable<ChatMessageViewModel> ChatMessages { get; set; }
    }
}
