using System;
using AmnesiaManager.Security;
using AmnesiaManager.Security.EncryptedValue;
using Newtonsoft.Json;

namespace AmnesiaManager.Models
{
    public class PasswordModel
    {
        #region Public Properties
        [JsonProperty("guid")] public Guid Guid { get; set; }
        [JsonProperty("label")] public string? Label { get; set; }
        [JsonProperty("login")] public string? Login { get; set; }
        [JsonProperty("password")] public EncryptedString Password { get; set; }
        #endregion

        #region Constructor
        public PasswordModel(EncryptedString password)
        {
            Password = password;
            Guid = Guid.NewGuid();
        }
        #endregion
    }
}