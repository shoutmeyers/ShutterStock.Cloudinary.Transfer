using Microsoft.Extensions.Logging;
using ShutterStock.ApiClient;
using ShutterStock.Cloudinary.Transfer.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ShutterStock.Cloudinary.Transfer
{
    public class RecurseFileStructure : IRecurseFileStructure
    {
        private readonly ILogger<RecurseFileStructure> _logger;

        public RecurseFileStructure(ILogger<RecurseFileStructure> logger)
        {
            _logger = logger;
        }

        public void TraverseDirectory(DirectoryInfo directoryInfo, string subscriptionId, IShutterStockApiClient client)
        {
            var subdirectories = directoryInfo.EnumerateDirectories();

            foreach (var subdirectory in subdirectories)
            {
                TraverseDirectory(subdirectory, subscriptionId, client);
            }

            var files = directoryInfo.EnumerateFiles();

            foreach (var file in files)
            {
                HandleFile(file, subscriptionId, client);
            }
        }

        private void HandleFile(FileInfo file, string subscriptionId, IShutterStockApiClient client)
        {
            _logger.LogInformation($"FILE: {file.Name}.");

            // extract id from filename
            var imageId = RegexUtil.ParseId(file.Name);

            if (!imageId.HasValue) return;

            _logger.LogInformation($"Image idenfifier: {imageId.Value}");

            try
            {
                var newFile = $@"{file.DirectoryName}\{imageId.Value}{file.Extension}";

                if (File.Exists(newFile))
                {
                    _logger.LogWarning($"{imageId.Value}{file.Extension} bestaat al.");
                    return;
                }

                var metaData = client.GetImage(imageId.Value);
                _logger.LogInformation(metaData.Description);

                var lines = new List<string> { $"Naam: {metaData.Name}", $"Omschrijving: {metaData.Description}", $"Image type: {metaData.ImageType}", " ", "Keywords:" };
                lines.AddRange(metaData.Keywords);

                File.WriteAllLines($@"{file.DirectoryName}\{Path.GetFileNameWithoutExtension(file.Name)}.txt", lines);

                // test purpose only
                var destinationFile = $@"{file.DirectoryName}\{imageId.Value}{file.Extension}";

                try
                {
                    file.CopyTo(destinationFile, true);
                }
                catch (IOException iox)
                {
                    _logger.LogError(iox.Message);
                }

                var license = client.GetLicense(imageId.Value, subscriptionId);
                _logger.LogInformation(license.Data.FirstOrDefault()?.ImageId);

                foreach (var license1 in license.Data)
                {
                    if (!string.IsNullOrEmpty(license1.Download.Url) && !string.IsNullOrEmpty(subscriptionId))
                    {
                        var image = $@"{file.DirectoryName}\shutterstock.{imageId.Value}{file.Extension}";

                        using (var webclient = new WebClient())
                        {
                            webclient.DownloadFile(license1.Download.Url, image);
                        }
                    }
                    else
                    {
                        _logger.LogError($"Image {file.Name} werd niet gevonden.");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}
