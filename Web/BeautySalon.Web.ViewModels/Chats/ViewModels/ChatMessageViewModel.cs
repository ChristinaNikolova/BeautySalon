namespace BeautySalon.Web.ViewModels.Chats.ViewModels
{
    using System;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ChatMessageViewModel : ClientChatViewModel, IMapFrom<ChatMessage>
    {
        public string Content { get; set; }

        public string ReceiverUsername { get; set; }

        public string ReceiverPicture { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FormattedDate
             => string.Format(
                 GlobalConstants.DateTimeFormat,
                 TimeZoneInfo.ConvertTimeFromUtc(this.CreatedOn, TimeZoneInfo.FindSystemTimeZoneById(GlobalConstants.LocalTimeZone)));
    }
}
