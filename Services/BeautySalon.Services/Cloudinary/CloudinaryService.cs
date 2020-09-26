namespace BeautySalon.Services.Cloudinary
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;
        private readonly string defaultProfilePicUrl = GlobalConstants.DefaultUserProfilePicture;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploudAsync(IFormFile image, string imageName)
        {
            if (image == null || !this.IsFileValid(image))
            {
                return this.defaultProfilePicUrl;
            }

            byte[] destinationImage;

            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                destinationImage = memoryStream.ToArray();
            }

            using (var ms = new MemoryStream(destinationImage))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageName, ms),
                    PublicId = imageName,
                };

                var uploadResult = this.cloudinary.UploadAsync(uploadParams);
                return uploadResult.Result.SecureUri.AbsoluteUri;
            }
        }

        public bool IsFileValid(IFormFile image)
        {
            bool isImageValid = true;

            string[] validTypes = new string[]
            {
                "image/x-png", "image/gif", "image/jpeg", "image/jpg", "image/png", "image/gif", "image/svg",
            };

            if (validTypes.Contains(image.ContentType) == false)
            {
                isImageValid = false;
            }

            return isImageValid;
        }
    }
}
