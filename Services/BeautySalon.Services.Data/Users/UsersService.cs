namespace BeautySalon.Services.Data.Users
{
    using System;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Models.Enums;
    using BeautySalon.Services.Cloudinary;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly IRepository<SkinType> skinTypesRepository;
        private readonly ICloudinaryService cloudinaryService;

        public UsersService(IRepository<ApplicationUser> usersRepository, IRepository<SkinType> skinTypesRepository, ICloudinaryService cloudinaryService)
        {
            this.usersRepository = usersRepository;
            this.skinTypesRepository = skinTypesRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<ApplicationUser> UpdateUserProfileAsync(string id, string username, string firstName, string lastName, string address, string phoneNumber, string gender, string skinTypeName, string isSkinSensitive, IFormFile picture)
        {
            var user = await this.GetUserByIdAsync(id);

            var newSkinType = await this.GetUserSkinTypeByNameAsync(skinTypeName);

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Address = address;
            user.PhoneNumber = phoneNumber;
            user.Gender = Enum.Parse<Gender>(gender);
            user.SkinTypeId = newSkinType.Id;
            user.IsSkinSensitive = isSkinSensitive == "Yes";

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
    }
}
