using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Cloudinary.ApiClient
{
    public class CloudinaryApiClient : ICloudinaryApiClient
    {
        private static readonly Account Account = new Account(
            "dqno4ddfi",
            "167628971918818",
            "4p-RR2J08E_abR6DkoDbWYlgA10"); // move this to config file

        private readonly CloudinaryDotNet.Cloudinary _cloudinary = new CloudinaryDotNet.Cloudinary(Account);

        private readonly ILogger<CloudinaryApiClient> _logger;

        public CloudinaryApiClient(ILogger<CloudinaryApiClient> logger)
        {
            _logger = logger;
        }

        public async Task<object> CreateImage(byte[] byteArray, string filename, string path)
        {
            try
            {
                Stream stream = new MemoryStream(byteArray);
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(filename, stream),
                    PublicId = $"{path}/{Guid.NewGuid()}"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                return new
                {
                    Uri = uploadResult.SecureUri.AbsoluteUri,
                    uploadResult.PublicId,
                    Name = Path.GetFileName(uploadResult.SecureUri.AbsoluteUri)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
