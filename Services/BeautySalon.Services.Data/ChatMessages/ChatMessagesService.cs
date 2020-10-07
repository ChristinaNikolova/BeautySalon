namespace BeautySalon.Services.Data.ChatMessages
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;

    public class ChatMessagesService : IChatMessagesService
    {
        private readonly IRepository<ChatMessage> chatMessagesRepository;

        public ChatMessagesService(IRepository<ChatMessage> chatMessagesRepository)
        {
            this.chatMessagesRepository = chatMessagesRepository;
        }

        public async Task<ChatMessage> CreateAsync(string content, string receiverId, string senderId)
        {
            var newChatMessage = new ChatMessage
            {
                Content = content,
                SenderId = senderId,
                ReceiverId = receiverId,
            };

            await this.chatMessagesRepository.AddAsync(newChatMessage);
            await this.chatMessagesRepository.SaveChangesAsync();

            return newChatMessage;
        }
    }
}
