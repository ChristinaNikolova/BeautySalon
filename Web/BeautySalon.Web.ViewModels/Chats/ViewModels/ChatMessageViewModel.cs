﻿namespace BeautySalon.Web.ViewModels.Chats.ViewModels
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ChatMessageViewModel : IMapFrom<ChatMessage>
    {
        public string Content { get; set; }

        public string SenderUsername { get; set; }

        public string SenderPicture { get; set; }

        public string ReceiverUsername { get; set; }

        public string ReceiverPicture { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FormattedDate
            => string.Format("{0:g}", this.CreatedOn);
    }
}
