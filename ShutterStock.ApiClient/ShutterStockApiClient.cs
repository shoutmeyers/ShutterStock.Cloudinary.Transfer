using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace ShutterStock.ApiClient
{
    public class ShutterStockApiClient : IShutterStockApiClient
    {
        private readonly IConfigurationRoot _config;

        private readonly ILogger<ShutterStockApiClient> _logger;

        public ShutterStockApiClient(ILogger<ShutterStockApiClient> logger, IConfigurationRoot config)
        {
            _logger = logger;
            _config = config;
        }

        public IRestResponse Authenticate()
        {
            var client = new RestClient(_config.GetSection("ShutterStock")["BaseUrl"]);

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
            var client = new RestClient($"{_config.GetSection("ShutterStock")["BaseUrl"]}/user");

            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + _config.GetSection("ShutterStock")["Bearer"], ParameterType.HttpHeader);

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
            var client = new RestClient($"{_config.GetSection("ShutterStock")["BaseUrl"]}/user/subscriptions");

            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + _config.GetSection("ShutterStock")["Bearer"], ParameterType.HttpHeader);

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
            var client = new RestClient($"{_config.GetSection("ShutterStock")["BaseUrl"]}/images/licenses");

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
            request.AddParameter("subscription_id", _config.GetSection("ShutterStock")["SubscriptionId"]);
            request.AddParameter("Authorization", "Bearer " + _config.GetSection("ShutterStock")["Bearer"], ParameterType.HttpHeader);

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
            var client = new RestClient($"{_config.GetSection("ShutterStock")["BaseUrl"]}/images/{id}");

            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + _config.GetSection("ShutterStock")["Bearer"], ParameterType.HttpHeader);

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
