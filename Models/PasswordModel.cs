using Newtonsoft.Json;

namespace AmnesiaManager.Models
{
    internal class PasswordModel
    {
        [JsonProperty("label")] public string? Label { get; set; }
        [JsonProperty("login")] public string? Login { get; set; }
        [JsonProperty("password")] public string? Password { get; set; }
    }
}
