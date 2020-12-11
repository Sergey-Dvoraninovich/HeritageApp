using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HeritageWebApplication.Services
{
    public interface IImageService
    {
        Task SaveImage(IFormFile file, string filePath);
    }
}