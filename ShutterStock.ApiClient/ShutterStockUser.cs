using Newtonsoft.Json;

namespace ShutterStock.ApiClient
{
    public class ShutterStockUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("contributor_id")]
        public string ContributorId { get; set; }
    }
}
