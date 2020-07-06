using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShutterStock.ApiClient
{
    public class ShutterStockSubscription
    {
        [JsonProperty("data")]
        public List<Datum> Data { get; set; }
    }

    public class Formats
    {
        [JsonProperty("size")]
        public string Size { get; set; }
        [JsonProperty("format")]
        public string Format { get; set; }
        [JsonProperty("media_type")]
        public string MediaType { get; set; }
        [JsonProperty("min_resolution")]
        public int MinResolution { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class Datum
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("expiration_time")]
        public DateTime ExpirationTime { get; set; }
        [JsonProperty("license")]
        public string License { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("formats")]
        public List<Formats> Formats { get; set; }

    }
}
