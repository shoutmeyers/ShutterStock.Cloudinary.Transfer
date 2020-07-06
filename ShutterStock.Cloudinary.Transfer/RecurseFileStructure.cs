using ShutterStock.ApiClient;
using System;
using System.IO;
using ShutterStock.Cloudinary.Transfer.Util;

namespace ShutterStock.Cloudinary.Transfer
{
    public class RecurseFileStructure
    {
        public void TraverseDirectory(DirectoryInfo directoryInfo, IShutterStockApiClient client)
        {
            var subdirectories = directoryInfo.EnumerateDirectories();

            foreach (var subdirectory in subdirectories)
            {
                Console.WriteLine($"FOLDER: {subdirectory.Name}.");
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
            Console.WriteLine(file.DirectoryName);
            Console.WriteLine($"FILE: { file.Name}.");

            // extract id from filename
            var imageId = RegexUtil.ParseId(file.Name);

            if (!imageId.HasValue) return;

            Console.WriteLine(imageId.Value);

            try
            {
                var metaData = client.GetImage(imageId.Value);
                var license = client.GetLicense(imageId.Value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
