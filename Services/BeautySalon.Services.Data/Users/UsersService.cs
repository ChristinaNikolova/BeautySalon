﻿namespace BeautySalon.Services.Data.Users
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Models.Enums;
    using BeautySalon.Services.Cloudinary;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly IRepository<SkinProblem> skinProblemsRepository;
        private readonly IRepository<ClientSkinProblem> clientSkinProblemsRepository;
        private readonly IRepository<Card> cardsRepository;
        private readonly ICloudinaryService cloudinaryService;

        public UsersService(
            IRepository<ApplicationUser> usersRepository,
            IRepository<SkinProblem> skinProblemsRepository,
            IRepository<ClientSkinProblem> clientSkinProblemsRepository,
            IRepository<Card> cardsRepository,
            ICloudinaryService cloudinaryService)
        {
            this.usersRepository = usersRepository;
            this.skinProblemsRepository = skinProblemsRepository;
            this.clientSkinProblemsRepository = clientSkinProblemsRepository;
            this.cardsRepository = cardsRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<ApplicationUser> UpdateUserProfileAsync(string id, string username, string firstName, string lastName, string address, string phoneNumber, string gender, IFormFile newPicture)
        {
            var user = await this.GetUserByIdAsync(id);

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Address = address;
            user.PhoneNumber = phoneNumber;
            user.Gender = Enum.Parse<Gender>(gender);

            if (newPicture != null)
            {
                string newPictureAsUrl = await this.cloudinaryService.UploudAsync(newPicture, username);
                user.Picture = newPictureAsUrl;
            }

            await this.usersRepository.SaveChangesAsync();

            return user;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await this.usersRepository
                            .All()
                            .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddSkinTypeDataAsync(string userId, bool isSkinSensitive, string skinTypeId, string[] skinProblems)
        {
            var user = await this.usersRepository
                .All()
                .FirstOrDefaultAsync(u => u.Id == userId);

            user.IsSkinSensitive = isSkinSensitive;
            user.SkinTypeId = skinTypeId;

            if (skinProblems != null)
            {
                await this.AddSkinProblemsAsync(userId, skinProblems, user);
            }

            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();
        }

        public async Task<T> GetUserDataAsync<T>(string userId)
        {
            var userData = await this.usersRepository
                .All()
                .Where(u => u.Id == userId)
                .To<T>()
                .FirstOrDefaultAsync();

            return userData;
        }

        public async Task<bool> HasSubscriptionCardAsync(string userId)
        {
            var hasAlreadyCard = await this.cardsRepository
                 .All()
                 .AnyAsync(c => c.ClientId == userId && c.EndDate >= DateTime.UtcNow);

            return hasAlreadyCard;
        }

        public async Task<T> GetUserCardAsync<T>(string userId)
        {
            var card = await this.cardsRepository
                .All()
                .Where(c => c.ClientId == userId && c.EndDate >= DateTime.UtcNow)
                .To<T>()
                .FirstOrDefaultAsync();

            return card;
        }

        public async Task<string> GetUsernameByIdAsync(string clientId)
        {
            var username = await this.usersRepository
                .All()
                .Where(u => u.Id == clientId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();

            return username;
        }

        private async Task RemoveExistingClientSkinProblemsAsync(string userId)
        {
            var hasClientAlreadySkinProblemsAsync = await this.clientSkinProblemsRepository
                                    .All()
                                    .AnyAsync(csp => csp.ClientId == userId);

            if (hasClientAlreadySkinProblemsAsync)
            {
                var clientSkinProblems = await this.clientSkinProblemsRepository
                    .All()
                    .Where(csp => csp.ClientId == userId)
                    .ToListAsync();

                foreach (var clientSkinProblemToRemove in clientSkinProblems)
                {
                    this.clientSkinProblemsRepository.Delete(clientSkinProblemToRemove);
                }

                await this.clientSkinProblemsRepository.SaveChangesAsync();
            }
        }

        private async Task AddSkinProblemsAsync(string userId, string[] skinProblems, ApplicationUser user)
        {
            await this.RemoveExistingClientSkinProblemsAsync(userId);

            foreach (var skinProblemName in skinProblems)
            {
                var skinProblem = await this.skinProblemsRepository
                    .All()
                    .FirstOrDefaultAsync(sp => sp.Name == skinProblemName);

                var clientSkinProblem = new ClientSkinProblem()
                {
                    SkinProblem = skinProblem,
                    Client = user,
                };

                user.ClientSkinProblems.Add(clientSkinProblem);
            }
        }
    }
}
