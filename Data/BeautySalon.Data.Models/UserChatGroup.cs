namespace BeautySalon.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Data.Common.Models;

    public class UserChatGroup : BaseDeletableModel<string>
    {
        public UserChatGroup()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string ChatGroupId { get; set; }

        public ChatGroup ChatGroup { get; set; }

        [Required]
        public string ClientId { get; set; }

        public ApplicationUser Client { get; set; }

        [Required]
        public string AdminId { get; set; }

        public ApplicationUser Admin { get; set; }
    }
}
