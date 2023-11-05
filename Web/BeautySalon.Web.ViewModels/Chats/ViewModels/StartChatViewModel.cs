namespace BeautySalon.Web.ViewModels.Chats.ViewModels
{
    using System.Collections.Generic;

    public class StartChatViewModel
    {
        public string CurrentUserUsername { get; set; }

        public string SenderUsername { get; set; }

        public string ReceiverUsername { get; set; }

        public string GroupName { get; set; }

        public IEnumerable<ChatMessageViewModel> ChatMessages { get; set; }
    }
}
