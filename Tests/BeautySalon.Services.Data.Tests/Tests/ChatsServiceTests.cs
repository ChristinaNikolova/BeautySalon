﻿namespace BeautySalon.Services.Data.Tests.Tests
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

    public class ChatsServiceTests : BaseServiceTests
    {
        private readonly ApplicationUser sender;
        private readonly ApplicationUser receiver;

        private readonly Mock<IRepository<ChatMessage>> chatMessagesRepository;
        private readonly Mock<IRepository<UserChatGroup>> userChatGroupsRepository;

        public ChatsServiceTests()
        {
            this.chatMessagesRepository = new Mock<IRepository<ChatMessage>>();
            this.userChatGroupsRepository = new Mock<IRepository<UserChatGroup>>();

            this.sender = new ApplicationUser() { Id = "1" };
            this.receiver = new ApplicationUser() { Id = "2" };
        }

        [Fact]
        public async Task CheckCreatingUsersGroupAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ChatMessage>(db);
            var chatGroupRepository = new EfDeletableEntityRepository<ChatGroup>(db);
            var userChatGroupsRepository = new EfDeletableEntityRepository<UserChatGroup>(db);

            var service = new ChatsService(
                repository,
                chatGroupRepository,
                userChatGroupsRepository);

            var firstChatGroupId = await service.CreateUsersGroupAsync(this.sender, this.receiver, "chat name");
            var secondChatGroupId = await service.CreateUsersGroupAsync(this.sender, this.receiver, "chat name");

            Assert.NotNull(firstChatGroupId);
            Assert.NotNull(secondChatGroupId);
            Assert.EndsWith(firstChatGroupId, secondChatGroupId);
        }

        [Fact]
        public async Task CheckGettingGroupIdAsync()
        {
            ApplicationDbContext db = GetDb();

            var chatGroupRepository = new EfDeletableEntityRepository<ChatGroup>(db);

            var service = new ChatsService(
                this.chatMessagesRepository.Object,
                chatGroupRepository,
                this.userChatGroupsRepository.Object);

            var firstGroupId = await service.GetGroupIdAsync("chat name");

            await service.CreateUsersGroupAsync(this.sender, this.receiver, "chat name");
            var secondGroupId = await service.GetGroupIdAsync("chat name");

            Assert.Null(firstGroupId);
            Assert.NotNull(secondGroupId);
        }

        [Fact]
        public async Task CheckGettingGroupsOldMessagesAsync()
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
        public async Task CheckGettingTheReceiverAsync()
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
        public async Task CheckGettingWaitingForAnswerMessagesAsync()
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
        public async Task CheckIfThereAreNewMessageAsync()
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
        public async Task CheckSendingMessageSenderClientAsync()
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
        public async Task CheckSendingMessageSenderAdminAsync()
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

        public class TestChatModel : IMapFrom<ChatMessage>
        {
            public string Id { get; set; }
        }
    }
}
