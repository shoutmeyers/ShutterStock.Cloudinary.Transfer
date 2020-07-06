using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShutterStock.ApiClient
{
    // 1. Get code
    // 2. OAuth get bearer token
    // 3. Get image Id (from directory)
    // 4. Get license for image POST /v2/images/licenses and download image
    //{
    //"image_id": "59656357",
    //"format": "jpg",
    //"metadata": {
    //    "customer_id": "12345",
    //    "geo_location": "US",
    //    "number_viewed": "15",
    //    "search_term": "dog"
    //}
    //}
    // 5. Get metadata GET /v2/images/{id}

    public class VectorEps
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("format")]
        public string Format { get; set; }
        [JsonProperty("is_licensable")]
        public bool IsLicensable { get; set; }

    }

    public class HugeJpg
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("dpi")]
        public int Dpi { get; set; }
        [JsonProperty("file_size")]
        public int FileSize { get; set; }
        [JsonProperty("format")]
        public string Format { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("is_licensable")]
        public bool IsLicensable { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }

    }

    public class Preview
    {
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }

    }

    public class SmallThumb
    {
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }

    }

    public class LargeThumb
    {
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }

    }

    public class HugeThumb
    {
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }

    }

    public class Preview1000
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }

    }

    public class Preview1500
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }

    }

    public class Assets
    {
        [JsonProperty("vector_eps")]
        public VectorEps VectorEps { get; set; }
        [JsonProperty("huge_jpg")]
        public HugeJpg HugeJpg { get; set; }
        [JsonProperty("preview")]
        public Preview Preview { get; set; }
        [JsonProperty("small_thumb")]
        public SmallThumb SmallThumb { get; set; }
        [JsonProperty("large_thumb")]
        public LargeThumb LargeThumb { get; set; }
        [JsonProperty("huge_thumb")]
        public HugeThumb HugeThumb { get; set; }
        [JsonProperty("preview_1000")]
        public Preview1000 Preview1000 { get; set; }
        [JsonProperty("preview_1500")]
        public Preview1500 Preview1500 { get; set; }

    }

    public class Category
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

    }

    public class Contributor
    {
        [JsonProperty("id")]
        public string Id { get; set; }

    }

    public class ModelReleas
    {
        [JsonProperty("id")]
        public string Id { get; set; }

    }

    public class ShutterStockResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("added_date")]
        public string AddedDate { get; set; }
        [JsonProperty("aspect")]
        public double Aspect { get; set; }
        [JsonProperty("assets")]
        public Assets Assets { get; set; }
        [JsonProperty("categories")]
        public List<Category> Categories { get; set; }
        [JsonProperty("contributor")]
        public Contributor Contributor { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("image_type")]
        public string ImageType { get; set; }
        [JsonProperty("is_adult")]
        public bool IsAdult { get; set; }
        [JsonProperty("is_editorial")]
        public bool IsEditorial { get; set; }
        [JsonProperty("is_illustration")]
        public bool IsIllustration { get; set; }
        [JsonProperty("has_model_release")]
        public bool HasModelRelease { get; set; }
        [JsonProperty("has_property_release")]
        public bool HasPropertyRelease { get; set; }
        [JsonProperty("keywords")]
        public List<string> Keywords { get; set; }
        [JsonProperty("media_type")]
        public string Name { get; set; }
        [JsonProperty("model_releases")]
        public List<ModelReleas> ModelReleases { get; set; }
        [JsonProperty("original_filename")]
        public string OriginalFilename { get; set; }
    }

}
