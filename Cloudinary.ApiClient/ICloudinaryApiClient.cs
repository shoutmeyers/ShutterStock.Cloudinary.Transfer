using System.Threading.Tasks;

namespace Cloudinary.ApiClient
{
    public interface ICloudinaryApiClient
    {
        Task<object> CreateImage(byte[] byteArray, string filename, string path);
    }
}
