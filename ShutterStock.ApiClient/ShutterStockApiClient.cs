using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace ShutterStock.ApiClient
{
    public class ShutterStockApiClient : IShutterStockApiClient
    {
        private string Bearer { get; } =
            "v2/eXRRcmxYcWl5RDcyMjlHendGbHpJUkFMVlJFZVdRZlYvMjcxMzk5MTc4L2N1c3RvbWVyLzMvVDI0Qi12bDExaTZQVWhRY1pjTjlvS1ZvRWZyZWFtM216UUM2SjRUMktYMXdKbVZ4X0JQbl9LSnpVelROSTU5ZTNBcDFTQW5iWXF0YVRURUlCdGJLVGNtYUpYMXkzU3N4Q1NqZjkzQ3VWVF91bzBvTmwzLXJBT2EtMUxtREFGNVpBUWRJX1V0YjJpci0wbU9JbUZSdk9IOUMzN1JpdUpITGlpMVdYZW1HUk5mM0x1ZG1zTE8tazcyUmN3TnFmVGY0RTVWU1lhdWo3TGFFcE42MV9LS3l2dw";

        private string BaseUrl { get; } = "https://api-sandbox.shutterstock.com/v2";

        private string SubscriptionId { get; } = "";

        private readonly ILogger<ShutterStockApiClient> _logger;

        public ShutterStockApiClient(ILogger<ShutterStockApiClient> logger)
        {
            _logger = logger;
        }

        public IRestResponse Authenticate()
        {
            var client = new RestClient(BaseUrl);

            var request = new RestRequest { Method = Method.POST };

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "application/json");

            request.AddParameter("grant_type", "client_credentials");

            client.Authenticator = new HttpBasicAuthenticator("client-app", "secret");

            var response = client.Execute(request);

            return response;
        }

        public ShutterStockUser GetUser()
        {
            var client = new RestClient($"{BaseUrl}/user");

            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + Bearer, ParameterType.HttpHeader);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ShutterStockUser>(response.Content);
            }

            _logger.LogError(response.Content);
            throw new Exception(response.Content);
        }

        public ShutterStockSubscription GetSubscriptions()
        {
            var client = new RestClient($"{BaseUrl}/user/subscriptions");

            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + Bearer, ParameterType.HttpHeader);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ShutterStockSubscription>(response.Content);
            }

            _logger.LogError(response.Content);
            throw new Exception(response.Content);
        }

        public ShutterStockLicenseResponse GetLicense(int id)
        {
            var client = new RestClient($"{BaseUrl}/images/licenses");

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            var apiInput = new ShutterStockLicensePost
            {
                Images = new List<Image>
                {
                    new Image
                    {
                        ImageId = id.ToString(),
                        Format = "jpg",
                        Size = "medium",
                        Price = 0,
                        Metadata = new Metadata
                        {
                            CustomerId = "12345", 
                            GeoLocation = "",
                            NumberViewed = "",
                            SearchTerm = ""
                        },
                        EditorialAcknowledgement = false
                    }
                }
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(apiInput), ParameterType.RequestBody);
            request.AddParameter("subscription_id", SubscriptionId);
            request.AddParameter("Authorization", "Bearer " + Bearer, ParameterType.HttpHeader);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ShutterStockLicenseResponse>(response.Content);
            }

            _logger.LogError(response.Content);
            throw new Exception(response.Content);
        }

        public ShutterStockResponse GetImage(int id)
        {
            var client = new RestClient($"{BaseUrl}/images/{id}");

            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + Bearer, ParameterType.HttpHeader);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ShutterStockResponse>(response.Content);
            }

            _logger.LogError(response.Content);
            throw new Exception(response.Content);
        }
    }
}
