using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShutterStock.ApiClient
{
    public class ShutterStockLicensePost
    {
        [JsonProperty("images")]
        public List<Image> Images { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }
        [JsonProperty("geo_location")]
        public string GeoLocation { get; set; }
        [JsonProperty("number_viewed")]
        public string NumberViewed { get; set; }
        [JsonProperty("search_term")]
        public string SearchTerm { get; set; }
    }

    public class Image
    {
        [JsonProperty("image_id")]
        public string ImageId { get; set; }
        [JsonProperty("format")]
        public string Format { get; set; }
        [JsonProperty("size")]
        public string Size { get; set; }
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("editorial_acknowledgement")]
        public bool? EditorialAcknowledgement { get; set; }
        [JsonProperty("price")]
        public int Price { get; set; }

    }

    public class Download
    {
        [JsonProperty("url")]
        public string Url { get; set; }

    }

    public class License
    {
        [JsonProperty("image_id")]
        public string ImageId { get; set; }
        [JsonProperty("download")]
        public Download Download { get; set; }
        [JsonProperty("allotment_charge")]
        public int AllotmentCharge { get; set; }

    }

    public class ShutterStockLicenseResponse
    {
        [JsonProperty("data")]
        public List<License> Data { get; set; }

    }
}
