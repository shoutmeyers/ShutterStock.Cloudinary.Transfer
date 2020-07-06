using RestSharp;

namespace ShutterStock.ApiClient
{
    public interface IShutterStockApiClient
    {
        IRestResponse Authenticate();

        ShutterStockUser GetUser();

        ShutterStockSubscription GetSubscriptions();

        ShutterStockLicenseResponse GetLicense(int id);

        ShutterStockResponse GetImage(int id);
    }
}
