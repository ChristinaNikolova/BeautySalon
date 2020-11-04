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
        private readonly IRepository<SkinType> skinTypesRepository;
        private readonly IRepository<SkinProblem> skinProblemsRepository;
        private readonly ICloudinaryService cloudinaryService;

        public UsersService(IRepository<ApplicationUser> usersRepository, IRepository<SkinType> skinTypesRepository, IRepository<SkinProblem> skinProblemsRepository, ICloudinaryService cloudinaryService)
        {
            this.usersRepository = usersRepository;
            this.skinTypesRepository = skinTypesRepository;
            this.skinProblemsRepository = skinProblemsRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<ApplicationUser> UpdateUserProfileAsync(string id, string username, string firstName, string lastName, string address, string phoneNumber, string gender, string skinTypeName, string isSkinSensitive, IFormFile picture)
        {
            var user = await this.GetUserByIdAsync(id);

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Address = address;
            user.PhoneNumber = phoneNumber;
            user.Gender = Enum.Parse<Gender>(gender);
            user.IsSkinSensitive = isSkinSensitive == "Yes";

            if (skinTypeName != null)
            {
                var newSkinType = await this.GetUserSkinTypeByNameAsync(skinTypeName);
                user.SkinTypeId = newSkinType.Id;
            }

            if (picture != null)
            {
                string newPicture = await this.cloudinaryService.UploudAsync(picture, username);
                user.Picture = newPicture;
            }

            await this.usersRepository.SaveChangesAsync();

            return user;
        }

        public async Task<SkinType> GetUserSkinTypeByIdAsync(string skinTypeId)
        {
            var skinType = await this.skinTypesRepository
                .All()
                .FirstOrDefaultAsync(st => st.Id == skinTypeId);

            return skinType;
        }

        public async Task<SkinType> GetUserSkinTypeByNameAsync(string skinTypeName)
        {
            var skinType = await this.skinTypesRepository
                .All()
                .FirstOrDefaultAsync(st => st.Name == skinTypeName);

            return skinType;
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

        public async Task AddSkinTypeData(string userId, bool isSkinSensitive, string skinTypeId, string[] skinProblems)
        {
            var user = await this.usersRepository
                .All()
                .FirstOrDefaultAsync(u => u.Id == userId);

            user.IsSkinSensitive = isSkinSensitive;
            user.SkinTypeId = skinTypeId;

            if (skinProblems != null)
            {
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

            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();
        }
    }
}
