namespace BeautySalon.CronJobs
{
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class DeleteChatMessages
    {
        //todo add interface
        private readonly IDeletableEntityRepository<ChatMessage> chatMessagesRepository;

        public DeleteChatMessages(IDeletableEntityRepository<ChatMessage> chatMessagesRepository)
        {
            this.chatMessagesRepository = chatMessagesRepository;
        }

        public async Task DeleteAsync()
        {
            var allChatMessages = await this.chatMessagesRepository
                .All()
                .ToListAsync();

            foreach (var chatMessage in allChatMessages)
            {
                this.chatMessagesRepository.HardDelete(chatMessage);
            }

            await this.chatMessagesRepository.SaveChangesAsync();
        }
    }
}
