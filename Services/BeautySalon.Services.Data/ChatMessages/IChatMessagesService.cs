namespace BeautySalon.Services.Data.ChatMessages
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;

    public interface IChatMessagesService
    {
        Task<ChatMessage> CreateAsync(string content, string receiverId, string senderId);
    }
}
