namespace BeautySalon.Services.Data.ChatMessages
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;

    public interface IChatService
    {
        Task<ChatMessage> CreateAsync(string content, string receiverId, string senderId);

        Task<T> CreateChatAsync<T>(string userId);
    }
}
