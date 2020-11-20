namespace BeautySalon.Services.Data.ChatMessages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeautySalon.Common;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ChatsService : IChatsService
    {
        private readonly IRepository<ChatMessage> chatMessagesRepository;
        private readonly IRepository<ChatGroup> chatGroupsRepository;
        private readonly IRepository<UserChatGroup> userChatGroupsRepository;
        private readonly IRepository<ApplicationUser> usersRepository;

        public ChatsService(
            IRepository<ChatMessage> chatMessagesRepository,
            IRepository<ChatGroup> chatGroupsRepository,
            IRepository<UserChatGroup> userChatGroupsRepository,
            IRepository<ApplicationUser> usersRepository)
        {
            this.chatMessagesRepository = chatMessagesRepository;
            this.chatGroupsRepository = chatGroupsRepository;
            this.userChatGroupsRepository = userChatGroupsRepository;
            this.usersRepository = usersRepository;
        }

        public async Task<string> CreateUsersGroup(ApplicationUser sender, ApplicationUser receiver, string groupName)
        {
            var isGroupAlreadyCreated = await this.chatGroupsRepository
                .All()
                .AnyAsync(cg => cg.Name == groupName);

            if (isGroupAlreadyCreated)
            {
                var groupId = this.chatGroupsRepository
                .All()
                .FirstOrDefault(cg => cg.Name == groupName)
                .Id;

                return groupId;
            }

            var chatGroup = new ChatGroup()
            {
                Name = groupName,
            };

            var userChatGroup = new UserChatGroup()
            {
                ChatGroupId = chatGroup.Id,
                Client = sender,
                Admin = receiver,
            };

            await this.chatGroupsRepository.AddAsync(chatGroup);
            await this.userChatGroupsRepository.AddAsync(userChatGroup);
            await this.chatGroupsRepository.SaveChangesAsync();
            await this.userChatGroupsRepository.SaveChangesAsync();

            return chatGroup.Id;
        }

        public async Task<string> GetGroupIdAsync(string groupName)
        {
            var group = await this.chatGroupsRepository
                .All()
                .FirstOrDefaultAsync(cg => cg.Name == groupName);

            if (group == null)
            {
                return null;
            }

            return group.Id;
        }

        public async Task<IEnumerable<T>> GetOldMessagesAsync<T>(string groupId)
        {
            var messages = await this.chatMessagesRepository
                .All()
                .Where(cm => cm.ChatGroup.Id == groupId)
                .OrderBy(cm => cm.CreatedOn)
                .To<T>()
                .ToListAsync();

            return messages;
        }

        public async Task<string> GetReceiverAsync(string groupId, string senderUsername)
        {
            var userChatGroup = await this.userChatGroupsRepository
                .All()
                .FirstOrDefaultAsync(ucg => ucg.ChatGroup.Id == groupId);

            var receiverId = string.Empty;

            if (userChatGroup.Client.UserName == senderUsername)
            {
                receiverId = userChatGroup.AdminId;
            }
            else
            {
                receiverId = userChatGroup.ClientId;
            }

            return receiverId;
        }

        public async Task<IEnumerable<T>> GetWaitingForAnswerClientNamesAsync<T>()
        {
            var clientNames = await this.chatMessagesRepository
                .All()
                .Where(cm => cm.WaitingForAnswerFromAdmin == true)
                .To<T>()
                .Distinct()
                .ToListAsync();

            return clientNames;
        }

        public async Task SendMessageAsync(string chatMessage, ApplicationUser sender, ApplicationUser receiver, string groupName)
        {
            var chatGroup = await this.chatGroupsRepository
                .All()
                .FirstOrDefaultAsync(cg => cg.Name == groupName);

            var message = new ChatMessage()
            {
                Content = chatMessage,
                Sender = sender,
                Receiver = receiver,
                ChatGroup = chatGroup,
            };

            if (sender.UserName == GlobalConstants.AdminName)
            {
                message.WaitingForAnswerFromAdmin = false;

                foreach (var oldMessage in chatGroup.ChatMessages)
                {
                    oldMessage.WaitingForAnswerFromAdmin = false;
                }
            }

            this.chatGroupsRepository.Update(chatGroup);
            await this.chatGroupsRepository.SaveChangesAsync();

            await this.chatMessagesRepository.AddAsync(message);
            await this.chatMessagesRepository.SaveChangesAsync();
        }
    }
}
