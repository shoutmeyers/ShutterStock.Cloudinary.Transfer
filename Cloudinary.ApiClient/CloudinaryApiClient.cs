using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Cloudinary.ApiClient
{
    public class CloudinaryApiClient : ICloudinaryApiClient
    {
        private static Account _account;

        private readonly CloudinaryDotNet.Cloudinary _cloudinary = new CloudinaryDotNet.Cloudinary(_account);

        private readonly ILogger<CloudinaryApiClient> _logger;

        public CloudinaryApiClient(ILogger<CloudinaryApiClient> logger, IConfigurationRoot config)
        {
            _logger = logger;
            var configuration = config;
            _account = new Account(
                configuration.GetSection("Cloudinary")["Cloud"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]);

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
