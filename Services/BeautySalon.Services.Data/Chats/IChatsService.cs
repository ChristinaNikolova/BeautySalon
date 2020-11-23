namespace BeautySalon.Services.Data.ChatMessages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;

    public interface IChatsService
    {
        Task<string> CreateUsersGroupAsync(ApplicationUser sender, ApplicationUser receiver, string groupName);

        Task<IEnumerable<T>> GetOldMessagesAsync<T>(string groupId);

        Task<string> GetReceiverAsync(string groupId, string senderUsername);

        Task<string> GetGroupIdAsync(string groupName);

        Task SendMessageAsync(string chatMessage, ApplicationUser sender, ApplicationUser receiver, string groupName);

        Task<IEnumerable<T>> GetWaitingForAnswerClientNamesAsync<T>();

        Task<bool> IsNewMessageAsync();
    }
}
