using ShutterStock.ApiClient;
using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using ShutterStock.Cloudinary.Transfer.Util;

namespace ShutterStock.Cloudinary.Transfer
{
    public class RecurseFileStructure : IRecurseFileStructure
    {
        private readonly ILogger<RecurseFileStructure> _logger;

        public RecurseFileStructure(ILogger<RecurseFileStructure> logger)
        {
            _logger = logger;
        }

        public void TraverseDirectory(DirectoryInfo directoryInfo, IShutterStockApiClient client)
        {
            var subdirectories = directoryInfo.EnumerateDirectories();

            foreach (var subdirectory in subdirectories)
            {
                _logger.LogInformation($"FOLDER: {subdirectory.Name}.");
                TraverseDirectory(subdirectory, client);
            }

            var files = directoryInfo.EnumerateFiles();

            foreach (var file in files)
            {
                HandleFile(file, client);
            }
        }

        void HandleFile(FileInfo file, IShutterStockApiClient client)
        {
            _logger.LogInformation(file.DirectoryName);
            _logger.LogInformation($"FILE: { file.Name}.");

            // extract id from filename
            var imageId = RegexUtil.ParseId(file.Name);

            if (!imageId.HasValue) return;

            _logger.LogInformation($"Image idenfifier: {imageId.Value}");

            try
            {
                var metaData = client.GetImage(imageId.Value);
                _logger.LogInformation(metaData.Description);

                var license = client.GetLicense(imageId.Value);
                _logger.LogInformation(license.Data.FirstOrDefault()?.ImageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}
