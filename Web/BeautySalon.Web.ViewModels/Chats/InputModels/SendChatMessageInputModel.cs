namespace BeautySalon.Web.ViewModels.Chats.InputModels
{
    public class SendChatMessageInputModel
    {
        public string ChatMessage { get; set; }

        public string SenderUsername { get; set; }

        public string ReceiverUsername { get; set; }

        public string GroupName { get; set; }
    }
}
