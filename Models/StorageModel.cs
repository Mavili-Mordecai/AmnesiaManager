using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmnesiaManager.Models
{
    internal class StorageModel
    {
        [JsonProperty("is_authenticated")] public bool IsAuthenticated { get; set; }
        [JsonProperty("passwords")] public List<PasswordModel>? Passwords { get; set; }
    }
}
