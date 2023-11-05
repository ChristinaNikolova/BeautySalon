namespace BeautySalon.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;

    public class ChatGroup : BaseDeletableModel<string>
    {
        public ChatGroup()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserChatGroups = new HashSet<UserChatGroup>();
            this.ChatMessages = new HashSet<ChatMessage>();
        }

        [Required]
        [MaxLength(DataValidation.ChatGroupNameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<UserChatGroup> UserChatGroups { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
