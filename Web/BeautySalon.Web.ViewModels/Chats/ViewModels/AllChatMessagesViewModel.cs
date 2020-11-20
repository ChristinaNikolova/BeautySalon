namespace BeautySalon.Web.ViewModels.Chats.ViewModels
{
    using System.Collections.Generic;

    public class AllChatMessagesViewModel
    {
        public IEnumerable<ChatMessageViewModel> ChatMessages { get; set; }
    }
}
