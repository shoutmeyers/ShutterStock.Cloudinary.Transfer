using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ShutterStock.ApiClient
{
    public class ShutterStockApiClient : IShutterStockApiClient
    {
        private readonly IConfigurationRoot _config;

        private readonly ILogger<ShutterStockApiClient> _logger;

        private string _bearer;

        public ShutterStockApiClient(ILogger<ShutterStockApiClient> logger, IConfigurationRoot config)
        {
            _logger = logger;
            _config = config;
        }

        public void Authorize()
        {
            var url =
                $"https://accounts.shutterstock.com/login?next=%2Foauth%2Fauthorize%3Fclient_id%3D{_config.GetSection("ShutterStock")["ConsumerKey"]}%26realm%3Dcustomer%26redirect_uri%3Dhttp%3A%2F%2Flocalhost%3A3000%26response_type%3Dcode%26scope%3Duser.view%20user.edit%20collections.view%20collections.edit%20licenses.view%20licenses.create%20earnings.view%20media.upload%20media.submit%20media.edit%20purchases.view%20reseller.view%20reseller.purchase%26state%3Dpoc";

            OpenUrl(url);
        }

        public void Authenticate(string shutterStockCode)
        {
            var client = new RestClient($"{_config.GetSection("ShutterStock")["BaseUrl"]}/oauth/access_token");

            var request = new RestRequest { Method = Method.POST };

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            var body = $"client_id={_config.GetSection("ShutterStock")["ConsumerKey"]}&client_secret={_config.GetSection("ShutterStock")["ConsumerSecret"]}&code={shutterStockCode}&grant_type=authorization_code&realm=customer&expires=false";

            request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var token = JsonConvert.DeserializeObject<ShutterStockToken>(response.Content);
                _bearer = token.AccessToken;
            }
            else
            {
                _logger.LogError(response.Content);
                throw new Exception(response.Content);
            }
        }

        public ShutterStockUser GetUser()
        {
            var client = new RestClient($"{_config.GetSection("ShutterStock")["BaseUrl"]}/user");

            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + _bearer, ParameterType.HttpHeader);

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
            request.AddParameter("Authorization", "Bearer " + _bearer, ParameterType.HttpHeader);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ShutterStockSubscription>(response.Content);
            }

            _logger.LogError(response.Content);
            throw new Exception(response.Content);
        }

        public ShutterStockLicenseResponse GetLicense(int id, string subscriptionId)
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
            request.AddParameter("subscription_id", subscriptionId);
            request.AddParameter("Authorization", "Bearer " + _bearer, ParameterType.HttpHeader);

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
            request.AddParameter("Authorization", "Bearer " + _bearer, ParameterType.HttpHeader);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ShutterStockResponse>(response.Content);
            }

            _logger.LogError(response.Content);
            throw new Exception(response.Content);
        }

        private static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
