using Newtonsoft.Json;

namespace AmnesiaManager.Models
{
    internal class PasswordModel
    {
        [JsonProperty("label")] public string? Label { get; set; }
        [JsonProperty("password")] public string? Password { get; set; }
        [JsonProperty("username")] public string? Username { get; set; }
    }
}
