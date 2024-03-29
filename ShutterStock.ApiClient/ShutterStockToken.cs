﻿using Newtonsoft.Json;

namespace ShutterStock.ApiClient
{
    public class ShutterStockToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}
