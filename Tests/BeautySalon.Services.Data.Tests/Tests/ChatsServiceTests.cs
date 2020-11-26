namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.ChatMessages;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ChatsServiceTests
    {
        private readonly ApplicationUser sender;
        private readonly ApplicationUser receiver;

        private readonly Mock<IRepository<ChatMessage>> chatMessagesRepository;
        private readonly Mock<IRepository<UserChatGroup>> userChatGroupsRepository;

        public ChatsServiceTests()
        {
            new MapperInitializationProfile();
            this.chatMessagesRepository = new Mock<IRepository<ChatMessage>>();
            this.userChatGroupsRepository = new Mock<IRepository<UserChatGroup>>();

            this.sender = new ApplicationUser()
            {
                Id = "1",
            };
            this.receiver = new ApplicationUser()
            {
                Id = "2",
            };
        }

        [Fact]
        public async Task CheckCreatingUsersGroupNonExistingYet()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ChatMessage>(db);
            var chatGroupRepository = new EfDeletableEntityRepository<ChatGroup>(db);
            var userChatGroupsRepository = new EfDeletableEntityRepository<UserChatGroup>(db);

            var service = new ChatsService(
                repository,
                chatGroupRepository,
                userChatGroupsRepository);

            var chatGroupId = await service.CreateUsersGroupAsync(this.sender, this.receiver, "chat name");

            Assert.NotNull(chatGroupId);
        }

        [Fact]
        public async Task CheckCreatingUsersGroupAlreadyExisting()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ChatMessage>(db);
            var chatGroupRepository = new EfDeletableEntityRepository<ChatGroup>(db);
            var userChatGroupsRepository = new EfDeletableEntityRepository<UserChatGroup>(db);

            var service = new ChatsService(
                repository,
                chatGroupRepository,
                userChatGroupsRepository);

            var chatGroupIdFirst = await service.CreateUsersGroupAsync(this.sender, this.receiver, "chat name");

            var chatGroupIdSecond = await service.CreateUsersGroupAsync(this.sender, this.receiver, "chat name");

            var expectedId = chatGroupIdFirst;

            Assert.NotNull(chatGroupIdSecond);
            Assert.Same(expectedId, chatGroupIdSecond);
        }

        [Fact]
        public async Task CheckGettingGroupIdExistCase()
        {
            ApplicationDbContext db = GetDb();

            var chatGroupRepository = new EfDeletableEntityRepository<ChatGroup>(db);

            var service = new ChatsService(
                this.chatMessagesRepository.Object,
                chatGroupRepository,
                this.userChatGroupsRepository.Object);

            await service.CreateUsersGroupAsync(this.sender, this.receiver, "chat name");

            var groupId = await service.GetGroupIdAsync("chat name");

            Assert.NotNull(groupId);
        }

        [Fact]
        public async Task CheckGettingGroupIdNonExistCase()
        {
            ApplicationDbContext db = GetDb();

            var chatGroupRepository = new EfDeletableEntityRepository<ChatGroup>(db);

            var service = new ChatsService(
                this.chatMessagesRepository.Object,
                chatGroupRepository,
                this.userChatGroupsRepository.Object);

            var groupId = await service.GetGroupIdAsync("chat name");

            Assert.Null(groupId);
        }

        [Fact]
        public async Task CheckGettingGroupsOldMessages()
        {
            ApplicationDbContext db = GetDb();

            var chatGroupRepository = new EfDeletableEntityRepository<ChatGroup>(db);
            var repository = new EfDeletableEntityRepository<ChatMessage>(db);
            var userChatGroupsRepository = new EfDeletableEntityRepository<UserChatGroup>(db);

            var service = new ChatsService(
                repository,
                chatGroupRepository,
                userChatGroupsRepository);

            await service.CreateUsersGroupAsync(this.sender, this.receiver, "chat name");

            var groupId = await service.GetGroupIdAsync("chat name");

            var firstChatMessage = new ChatMessage()
            {
                Id = Guid.NewGuid().ToString(),
                Receiver = this.receiver,
                Sender = this.sender,
                ChatGroupId = groupId,
            };

            var secondChatMessage = new ChatMessage()
            {
                Id = Guid.NewGuid().ToString(),
                Receiver = this.receiver,
                Sender = this.sender,
                ChatGroupId = groupId,
            };

            await repository.AddAsync(firstChatMessage);
            await repository.AddAsync(secondChatMessage);
            await repository.SaveChangesAsync();

            var messages = await service.GetOldMessagesAsync<TestChatModel>(groupId);

            Assert.Equal(2, messages.Count());
        }

        [Fact]
        public async Task CheckGettingTheReceiver()
        {
            ApplicationDbContext db = GetDb();

            var userChatGroupsRepository = new EfDeletableEntityRepository<UserChatGroup>(db);
            var chatGroupsRepository = new EfDeletableEntityRepository<ChatGroup>(db);

            var service = new ChatsService(
                this.chatMessagesRepository.Object,
                chatGroupsRepository,
                userChatGroupsRepository);

            var currentSender = new ApplicationUser()
            {
                Id = "20",
                UserName = "senderUsername",
            };

            var currentReceiver = new ApplicationUser()
            {
                Id = "21",
                UserName = "receiverUsername",
            };

            await service.CreateUsersGroupAsync(currentSender, currentReceiver, "chat name");

            var groupId = await service.GetGroupIdAsync("chat name");

            var firstReceiverId = await service.GetReceiverAsync(groupId, currentSender.UserName);
            var secondReceiverId = await service.GetReceiverAsync(groupId, currentReceiver.UserName);

            Assert.Equal(currentReceiver.Id, firstReceiverId);
            Assert.Equal(currentSender.Id, secondReceiverId);
        }

        [Fact]
        public async Task CheckGettingWaitingForAnswerMessages()
        {
            ApplicationDbContext db = GetDb();

            var chatGroupRepository = new EfDeletableEntityRepository<ChatGroup>(db);
            var repository = new EfDeletableEntityRepository<ChatMessage>(db);
            var userChatGroupsRepository = new EfDeletableEntityRepository<UserChatGroup>(db);

            var service = new ChatsService(
                repository,
                chatGroupRepository,
                userChatGroupsRepository);

            await service.CreateUsersGroupAsync(this.sender, this.receiver, "chat name");

            var groupId = await service.GetGroupIdAsync("chat name");

            var firstChatMessage = new ChatMessage()
            {
                Id = Guid.NewGuid().ToString(),
                Receiver = this.receiver,
                Sender = this.sender,
                ChatGroupId = groupId,
                WaitingForAnswerFromAdmin = false,
            };

            var secondChatMessage = new ChatMessage()
            {
                Id = Guid.NewGuid().ToString(),
                Receiver = this.receiver,
                Sender = this.sender,
                ChatGroupId = groupId,
            };

            await repository.AddAsync(firstChatMessage);
            await repository.AddAsync(secondChatMessage);
            await repository.SaveChangesAsync();

            var messages = await service.GetWaitingForAnswerClientNamesAsync<TestChatModel>();

            Assert.Single(messages);
        }

        [Fact]
        public async Task CheckIfThereAreNewMessage()
        {
            ApplicationDbContext db = GetDb();

            var chatGroupRepository = new EfDeletableEntityRepository<ChatGroup>(db);
            var repository = new EfDeletableEntityRepository<ChatMessage>(db);
            var userChatGroupsRepository = new EfDeletableEntityRepository<UserChatGroup>(db);

            var service = new ChatsService(
                repository,
                chatGroupRepository,
                userChatGroupsRepository);

            await service.CreateUsersGroupAsync(this.sender, this.receiver, "chat name");

            var groupId = await service.GetGroupIdAsync("chat name");

            var firstChatMessage = new ChatMessage()
            {
                Id = Guid.NewGuid().ToString(),
                Receiver = this.receiver,
                Sender = this.sender,
                ChatGroupId = groupId,
            };

            await repository.AddAsync(firstChatMessage);
            await repository.SaveChangesAsync();

            var isMessage = await service.IsNewMessageAsync();

            Assert.True(isMessage);
        }

        [Fact]
        public async Task CheckSendingMessageSenderClient()
        {
            ApplicationDbContext db = GetDb();

            var chatGroupRepository = new EfDeletableEntityRepository<ChatGroup>(db);
            var repository = new EfDeletableEntityRepository<ChatMessage>(db);
            var userChatGroupsRepository = new EfDeletableEntityRepository<UserChatGroup>(db);

            var service = new ChatsService(
                repository,
                chatGroupRepository,
                userChatGroupsRepository);

            var currentSender = new ApplicationUser()
            {
                Id = "20",
                UserName = "senderUsername",
            };

            var currentReceiver = new ApplicationUser()
            {
                Id = "21",
                UserName = "Admin",
            };

            await service.CreateUsersGroupAsync(currentSender, currentReceiver, "chat name");

            await service.SendMessageAsync("message content", currentSender, currentReceiver, "chat name");

            var message = await repository.All().FirstOrDefaultAsync();

            Assert.NotNull(repository);
            Assert.True(message.WaitingForAnswerFromAdmin);
        }

        [Fact]
        public async Task CheckSendingMessageSenderAdmin()
        {
            ApplicationDbContext db = GetDb();

            var chatGroupRepository = new EfDeletableEntityRepository<ChatGroup>(db);
            var repository = new EfDeletableEntityRepository<ChatMessage>(db);
            var userChatGroupsRepository = new EfDeletableEntityRepository<UserChatGroup>(db);

            var service = new ChatsService(
                repository,
                chatGroupRepository,
                userChatGroupsRepository);

            var currentSender = new ApplicationUser()
            {
                Id = "20",
                UserName = "senderUsername",
            };

            var currentReceiver = new ApplicationUser()
            {
                Id = "21",
                UserName = "Admin",
            };

            await service.CreateUsersGroupAsync(this.sender, this.receiver, "chat name");
            var groupId = await service.GetGroupIdAsync("chat name");

            var chatMessage = new ChatMessage()
            {
                Id = Guid.NewGuid().ToString(),
                Receiver = currentReceiver,
                Sender = currentSender,
                ChatGroupId = groupId,
            };

            await repository.AddAsync(chatMessage);
            await repository.SaveChangesAsync();

            await service.CreateUsersGroupAsync(currentSender, currentReceiver, "chat name");

            await service.SendMessageAsync("message content", currentReceiver, currentSender, "chat name");

            var message = await repository.All().FirstOrDefaultAsync();

            Assert.NotNull(repository);
            Assert.True(!message.WaitingForAnswerFromAdmin);
        }

        private static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            return db;
        }

        public class TestChatModel : IMapFrom<ChatMessage>
        {
            public string Id { get; set; }
        }
    }
}
