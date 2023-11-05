namespace BeautySalon.Services.Cloudinary
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        bool IsFileValid(IFormFile image);

        Task<string> UploudAsync(IFormFile image, string imageName);
    }
}
