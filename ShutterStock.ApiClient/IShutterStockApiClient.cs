namespace ShutterStock.ApiClient
{
    public interface IShutterStockApiClient
    {
        void Authorize();

        void Authenticate(string shutterStockCode);

        ShutterStockUser GetUser();

        ShutterStockSubscription GetSubscriptions();

        ShutterStockLicenseResponse GetLicense(int id, string subscriptionId);

        ShutterStockResponse GetImage(int id);
    }
}
