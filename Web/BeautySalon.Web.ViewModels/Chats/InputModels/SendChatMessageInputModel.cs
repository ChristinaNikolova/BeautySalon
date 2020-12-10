namespace BeautySalon.Web.ViewModels.Chats.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class SendChatMessageInputModel
    {
        //test!!!!!!!!
        [Required]
        public string ChatMessage { get; set; }

        [Required]
        public string SenderUsername { get; set; }

        [Required]
        public string ReceiverUsername { get; set; }

        [Required]
        public string GroupName { get; set; }
    }
}
