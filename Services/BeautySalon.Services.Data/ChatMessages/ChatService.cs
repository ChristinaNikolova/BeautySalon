namespace BeautySalon.Services.Data.ChatMessages
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ChatMessagesService : IChatMessagesService
    {
        private readonly IRepository<ChatMessage> chatMessagesRepository;
        private readonly IRepository<ApplicationUser> usersRepository;

        public ChatMessagesService(
            IRepository<ChatMessage> chatMessagesRepository,
            IRepository<ApplicationUser> usersRepository)
        {
            this.chatMessagesRepository = chatMessagesRepository;
            this.usersRepository = usersRepository;
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

        public async Task<T> CreateChatAsync<T>(string userId)
        {
            var user = await this.usersRepository
                .All()
                .Where(u => u.Id == userId)
                .To<T>()
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
