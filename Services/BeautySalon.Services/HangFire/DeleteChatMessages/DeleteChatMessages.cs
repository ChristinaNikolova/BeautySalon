namespace BeautySalon.Services.HangFire.DeleteChatMessages
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class DeleteChatMessages : IDeleteChatMessages
    {
        private readonly int days = 7;

        private readonly IDeletableEntityRepository<ChatMessage> chatMessagesRepository;

        public DeleteChatMessages(IDeletableEntityRepository<ChatMessage> chatMessagesRepository)
        {
            this.chatMessagesRepository = chatMessagesRepository;
        }

        public async Task DeleteAsync()
        {
            var messageToDelete = await this.chatMessagesRepository
                .All()
                .Where(m => m.CreatedOn.AddDays(this.days).Date <= DateTime.Today.Date)
                .ToListAsync();

            foreach (var chatMessage in messageToDelete)
            {
                this.chatMessagesRepository.HardDelete(chatMessage);
            }

            await this.chatMessagesRepository.SaveChangesAsync();
        }
    }
}
