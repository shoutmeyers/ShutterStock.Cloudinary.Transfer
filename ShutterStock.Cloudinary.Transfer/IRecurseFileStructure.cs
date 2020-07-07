using ShutterStock.ApiClient;
using System.IO;

namespace ShutterStock.Cloudinary.Transfer
{
    public interface IRecurseFileStructure
    {
        void TraverseDirectory(DirectoryInfo directoryInfo, string subscriptionId, IShutterStockApiClient client);
    }
}
