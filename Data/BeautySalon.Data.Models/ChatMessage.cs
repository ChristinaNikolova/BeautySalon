namespace BeautySalon.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Data.Common.Models;

    public class ChatMessage : BaseDeletableModel<string>
    {
        public ChatMessage()
        {
            this.Id = Guid.NewGuid().ToString();
            this.WaitingForAnswerFromAdmin = true;
        }

        [Required]
        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public virtual ApplicationUser Receiver { get; set; }

        [Required]
        public string ChatGroupId { get; set; }

        public virtual ChatGroup ChatGroup { get; set; }

        [Required]
        public string Content { get; set; }

        public bool WaitingForAnswerFromAdmin { get; set; }
    }
}
