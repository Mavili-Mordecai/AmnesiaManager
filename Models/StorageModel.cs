using Newtonsoft.Json;
using System.Collections.Generic;

namespace AmnesiaManager.Models
{
    internal class StorageModel
    {
        [JsonProperty("is_authenticated")] public bool IsAuthenticated { get; set; }
        [JsonProperty("passwords")] public List<PasswordModel>? Passwords { get; set; }
    }
}
