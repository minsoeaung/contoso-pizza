using System.Text.Json.Serialization;

namespace ContosoPizza.External.Contracts;

public class FacebookUserInfoResult
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("email")] public string Email { get; set; }

    [JsonPropertyName("picture")] public FacebookPicture FacebookPicture { get; set; }
}

public class FacebookPicture
{
    [JsonPropertyName("data")] public FacebookPictureData Data { get; set; }
}

public class FacebookPictureData
{
    [JsonPropertyName("height")] public long Height { get; set; }

    [JsonPropertyName("is_silhouette")] public bool IsSilhouette { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }

    [JsonPropertyName("width")] public long Width { get; set; }
}