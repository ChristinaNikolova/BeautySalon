namespace BeautySalon.Services.Data.Users
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
        private readonly ICloudinaryService cloudinaryService;

        public UsersService(
            IRepository<ApplicationUser> usersRepository,
            IRepository<SkinProblem> skinProblemsRepository,
            IRepository<ClientSkinProblem> clientSkinProblemsRepository,
            ICloudinaryService cloudinaryService)
        {
            this.usersRepository = usersRepository;
            this.skinProblemsRepository = skinProblemsRepository;
            this.clientSkinProblemsRepository = clientSkinProblemsRepository;
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

        public async Task<T> GetUserDataAsync<T>(string userId)
        {
            var userData = await this.usersRepository
                .All()
                .Where(u => u.Id == userId)
                .To<T>()
                .FirstOrDefaultAsync();

            return userData;
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

        public async Task<T> GetUserSkinDataAsync<T>(string userId)
        {
            var userSkinData = await this.usersRepository
                .All()
                .Where(u => u.Id == userId)
                .To<T>()
                .FirstOrDefaultAsync();

            return userSkinData;
        }

        public async Task<T> GetUsersSkinDataForProfilePageAsync<T>(string userId)
        {
            var userSkinData = await this.usersRepository
               .All()
               .Where(u => u.Id == userId)
               .To<T>()
               .FirstOrDefaultAsync();

            return userSkinData;
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
            foreach (var skinProblemName in skinProblems)
            {
                var skinProblem = await this.skinProblemsRepository
                    .All()
                    .FirstOrDefaultAsync(sp => sp.Name == skinProblemName);

                var isUserSkinProblemAlreadyAddedAsync = await this.clientSkinProblemsRepository
                    .All()
                    .AnyAsync(csp => csp.ClientId == userId && csp.SkinProblemId == skinProblem.Id);

                if (isUserSkinProblemAlreadyAddedAsync)
                {
                    continue;
                }

                await this.RemoveExistingClientSkinProblemsAsync(userId);

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
